using UnityEngine;
using UnityEngine.InputSystem;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    public InputActionAsset playerControls;
    Rigidbody playerBody;
    PlayerMovement playerMovement; // references PlayerMovement script

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    float slideTimer;
    float originalDrag;

    public float slideYScale;
    float startYScale;

    [Header("Input")]
    float horizontalInput;
    float verticalInput;
    
    InputAction moveAction;
    InputAction slideAction;
    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = playerControls.FindActionMap("Movement").FindAction("Move");
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        slideAction = playerControls.FindActionMap("Movement").FindAction("Slide");

        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        startYScale = playerObject.localScale.y;
        originalDrag = playerMovement.groundDrag;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        if (slideAction.WasPressedThisFrame() && verticalInput > 0) // if W is being pressed down, allow player to start slide
        {
            StartSlide();
        }
        if ((slideAction.WasReleasedThisFrame() && playerMovement.isSliding) || verticalInput <= 0) // if slide key is released, or W key is released, stop sliding
        {
            StopSlide();
        }
    }
    
    void FixedUpdate()
    {
        if (playerMovement.isSliding)
        {
            SlidingMovement();
        }
    }
    
    void StartSlide()
    {
        playerMovement.isSliding = true;
        playerObject.localScale = new Vector3(playerObject.localScale.x, slideYScale, playerObject.localScale.z);
        playerBody.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    void StopSlide()
    {
        if (playerObject != null)
        {
            playerMovement.isSliding = false;
            playerObject.localScale = new Vector3(playerObject.localScale.x, startYScale, playerObject.localScale.z);
            playerMovement.groundDrag = originalDrag;
        }
        
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (playerMovement.isCrouching) // avoid crouching and sliding at the same time
        {
            StopSlide();
            return;
        }
        
        if (!playerMovement.OnSlope() || playerBody.velocity.y > -0.1f) // player is not on slope, but moving
        {
            if (playerMovement.isGrounded)
            {
                playerBody.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
                slideTimer -= Time.deltaTime;
            }
        }  
        else
        {
            playerBody.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            playerBody.AddForce(Vector3.down * 80f, ForceMode.Force); // removes player bumping up and down on slope
        }

        if (slideTimer <= 0) // allows player to slide for limited time before slowing down
        {
            if (playerBody.velocity.magnitude > 12) // change speed to whatever the slide speed until it drops below the player speed
            {
                playerMovement.playerSpeed = playerBody.velocity.magnitude;
                playerMovement.groundDrag += 0.35f;
            }
            else
            {
                playerMovement.playerSpeed = playerMovement.normalSpeed;
                playerMovement.groundDrag += 1f;
            }
        }
    }
}
