using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] // header helps categorize public variables
    public float playerSpeed;
    public float slideSpeed;
    public float wallRunSpeed;
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    [HideInInspector] public float normalSpeed;

    // momentum variables
    float desiredMoveSpeed;
    float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float maxYSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float slopeJumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump; // default protection is private

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    public float normalYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround; // defines what is considered ground
    public bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;
    
    public Transform orientation;
    public InputActionAsset playerControls;
    public GameObject sword;
    Animator animatorObj;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerBody;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction crouchAction;
    Vector2 moveInput;

    public MovementState state;
    public enum MovementState
    {
        standing,
        moving,
        crouching,
        sliding,
        wallrunning,
        dashing,
        air

    }

    public bool isSliding;
    public bool isCrouching;
    public bool isWallRunning;
    public bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = playerControls.FindActionMap("Movement").FindAction("Move");
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        jumpAction = playerControls.FindActionMap("Movement").FindAction("Jump");
        crouchAction = playerControls.FindActionMap("Movement").FindAction("Crouch");

        playerBody = GetComponent<Rigidbody>();
        normalYScale = transform.localScale.y;
        normalSpeed = playerSpeed;

        animatorObj = sword.GetComponent<Animator>();

        playerBody.freezeRotation = true; // removes physics engine's control over the rigidbody
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // checks if player is grounded using a raycast to find the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        ControlSpeed();
        StateHandler();

        // handles drag
        if (isGrounded && state != MovementState.dashing)
        {
            playerBody.drag = groundDrag;
        }
        else
        {
            playerBody.drag = 0;
        }
        // Debug.Log("Speed: " + playerBody.velocity.magnitude);
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        if (jumpAction.IsPressed() && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // resets ability to jump after cooldown
        }
        if (crouchAction.IsPressed() && isGrounded && playerSpeed <= 12) // start crouch if player is still or moving at normal speed
        {
            isCrouching = true;
            playerSpeed = crouchSpeed;
            playerBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (crouchAction.WasReleasedThisFrame()) // end crouch
        {
            isCrouching = false;
            playerSpeed = normalSpeed;
            transform.localScale = new Vector3(transform.localScale.x, normalYScale, transform.localScale.z);
        }
        // Debug.Log(playerBody.velocity.magnitude);
    }
    MovementState lastState;
    bool keepMomentum;
    void StateHandler()
    {
        if (isDashing) // dashing state
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            animatorObj.SetBool("isMoving", true);
        }
        else if (playerBody.velocity.magnitude < 3 && horizontalInput == 0 && verticalInput == 0 && !isCrouching) // standing state
        {
            animatorObj.SetBool("isMoving", false);
            state = MovementState.standing;
            desiredMoveSpeed = normalSpeed;
        }
        else if (isWallRunning) // wall running state
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
            animatorObj.SetBool("isMoving", true);
        }
        else if (isSliding) // sliding state
        {
            animatorObj.SetBool("isMoving", true);
            state = MovementState.sliding;

            if (OnSlope() && playerBody.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = playerSpeed;
            }
        }
        else if (isCrouching) // crouched state
        {
            if (horizontalInput == 0 && verticalInput == 0)
            {
                animatorObj.SetBool("isMoving", false);
            }
            else
            {
                animatorObj.SetBool("isMoving", true);
            }
            
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z); // moves player down if they land into crouch
        }
        else if (isGrounded) // moving state
        {
            animatorObj.SetBool("isMoving", true);
            state = MovementState.moving;
            desiredMoveSpeed = normalSpeed;
        }
        else // air movement state
        {
            animatorObj.SetBool("isMoving", true);
            state = MovementState.air;
            desiredMoveSpeed = normalSpeed; // as of right now, dashing resets movement to normal speed
        }

        if (lastState != MovementState.crouching && state != MovementState.dashing && playerBody.velocity.magnitude > 3 && Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 1f) // allows smooth transition between speeds when not moving from crouching or dashing
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            playerSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - playerSpeed);
        float startValue = playerSpeed;
        float boostSpeed = speedIncreaseMultiplier;
        
        if (lastState == MovementState.dashing)
        {
            boostSpeed = dashSpeedChangeFactor;
        }

        while (time < difference)
        {
            playerSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference); // linear interpolatiion, gradually moves playerSpeed back to normal, instead of abruptly like before
            
            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * boostSpeed * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime * boostSpeed;
            }
            yield return null;
        }
        playerSpeed = desiredMoveSpeed;
    }

    void MovePlayer()
    {
        if (state == MovementState.dashing)
        {
            return;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; // gets direction of keyboard input relative to where player is facing
        
        if (OnSlope() && !exitingSlope) // exitingSlope fixes issues with not being able to jump on slope
        {
            playerBody.AddForce(GetSlopeMoveDirection(moveDirection) * playerSpeed * 20f, ForceMode.Force);
            if (playerBody.velocity.y > 0)
            {
                playerBody.AddForce(Vector3.down * 80f, ForceMode.Force); // keeps player from flying off slope
            }        
        }

        // determines what force to apply if player is grounded or not
        if (isGrounded)
        {
            playerBody.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            playerBody.AddForce(moveDirection.normalized * playerSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        if (!isWallRunning)
        {
            playerBody.useGravity = !OnSlope(); // gravity depends on if the player is on a slope, ignoring wallrunning
        }
    }

    void ControlSpeed()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (playerBody.velocity.magnitude > playerSpeed)
            {
                playerBody.velocity = playerBody.velocity.normalized * playerSpeed; // fixes slope speed being faster than ground speed
            }
        }
        
        else
        {
            Vector3 flatVelocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

            // clamps velocity if it exceeds speed limit
            if (flatVelocity.magnitude > playerSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * playerSpeed;
                playerBody.velocity = new Vector3(limitedVelocity.x, playerBody.velocity.y, limitedVelocity.z);
            }
        }

        if (maxYSpeed != 0 && playerBody.velocity.y > maxYSpeed)
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, maxYSpeed, playerBody.velocity.z);
        }
    }

    void Jump()
    {
        // resets vertical velocity
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);
        if (OnSlope())
        {
            playerBody.AddForce(transform.up * jumpForce * slopeJumpForce, ForceMode.Impulse);
        }
        else
        {
            playerBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ResetJump()
    {
        canJump = true;
        exitingSlope = false;
    }

    public bool OnSlope() // checks if player is on a sloped surface
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) // details of what the ray hit is stored into slopeHit using out
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return slopeAngle < maxSlopeAngle && slopeAngle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized; // projects move direction onto slope so player can move properly
    }
}

// issues rn: 
// player will slide on slope when crouched
// player can get stuck to edge of wall very rarely

// add fov option?
// senstivity