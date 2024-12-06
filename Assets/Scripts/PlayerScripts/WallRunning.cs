using UnityEngine;
using UnityEngine.InputSystem;

public class Wallrunning : MonoBehaviour
{
    [Header("WallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float maxWallRunTime;
    public float wallClimbSpeed;
    float wallRunTimer;

    [Header("Exiting")]
    public float exitWallTime;
    bool exitingWall;
    float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("Input")]
    private bool isRunningUpwards;
    private bool isRunningDownwards;
    float horizontalInput;
    float verticalInput;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction scaleWallAction;
    InputAction descendWallAction;

    Vector2 moveInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool isLeftWall;
    bool isRightWall;

    [Header("References")]
    public Transform orientation;
    public PlayerCamera playerCamera;
    public InputActionAsset playerControls;
    PlayerMovement playerMovement;
    Rigidbody playerBody;
    PlayerAudio playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = playerControls.FindActionMap("Movement").FindAction("Move");
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;
        
        jumpAction = playerControls.FindActionMap("Movement").FindAction("Jump");
        scaleWallAction = playerControls.FindActionMap("Movement").FindAction("Scale Wall");
        descendWallAction = playerControls.FindActionMap("Movement").FindAction("Descend Wall");

        playerMovement = GetComponent<PlayerMovement>();
        playerBody = GetComponent<Rigidbody>();
        playerAudio = GetComponent<PlayerAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void FixedUpdate()
    {
        if (playerMovement.isWallRunning)
            WallRunningMovement();
    }

    void CheckForWall()
    {
        isLeftWall = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall); // raycast from position, stores result in "_wallHit"
        isRightWall = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        isRunningUpwards = scaleWallAction.IsPressed();
        isRunningDownwards = descendWallAction.IsPressed();

        if((isLeftWall || isRightWall) && verticalInput > 0 && AboveGround() && !exitingWall) // must have wall on either side, be running (w key pressed) and above ground
        {
            if (!playerMovement.isWallRunning)
            {
                StartWallRun();
            }

            if (wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            if (wallRunTimer <= 0 && playerMovement.isWallRunning)
            {   
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }


            if (jumpAction.WasPressedThisFrame())
            {
                WallJump();
            }
        }
        else if (exitingWall) // exits wall run
        {
            if (playerMovement.isWallRunning)
            {
                StopWallRun();
            }
            if (exitWallTimer > 0) // timer for how long the player can be on the wall
            {
                exitWallTimer -= Time.deltaTime;
            }
            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            if (playerMovement.isWallRunning)
            {
                StopWallRun();
            }
        }
    }

    void WallRunningMovement()
    {
        playerBody.useGravity = useGravity;

        Vector3 wallNormal = isRightWall ? rightWallHit.normal : leftWallHit.normal; // have not used one of these in a good minute

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up); // returns the cross product, finding forward direction of wall

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) // switches direction to where the player is facing
        {
            wallForward = -wallForward;
        }

        playerBody.AddForce(wallForward * wallRunForce, ForceMode.Force); // applies forward force

        // applies upwards/downwards force
        if (isRunningUpwards)
        {
            playerBody.velocity = new Vector3(playerBody.velocity.x, wallClimbSpeed, playerBody.velocity.z);
        }
        if (isRunningDownwards)
        {   
            playerBody.velocity = new Vector3(playerBody.velocity.x, -wallClimbSpeed, playerBody.velocity.z);
        }

        // pushes player towards the wall
        if (!(isLeftWall && horizontalInput > 0) && !(isRightWall && horizontalInput < 0))
        {
            playerBody.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        if (useGravity)
        {
            playerBody.AddForce(transform.up * gravityCounterForce, ForceMode.Force); // weakens gravity by adding counterforce
        }
    }
    
    void StartWallRun()
    {
        wallRunTimer = maxWallRunTime;
        playerMovement.isWallRunning = true;
        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z);

        // camera effects
        playerCamera.DoFov(90f); // cameraEffect.DoFov(fieldOfView + 10);
        if (isLeftWall)
        {
            playerCamera.DoTilt(-5f);
        }
        if (isRightWall)
        {
            playerCamera.DoTilt(5f);
        }
    }

    void StopWallRun()
    {
        playerMovement.isWallRunning = false;
        playerCamera.DoFov(80f); // cameraEffect.DoFov(fieldOfView);
        playerCamera.DoTilt(0f);
    }

    void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        playerAudio.PlayPlayerAudio("jump");

        Vector3 wallNormal = isRightWall ? rightWallHit.normal : leftWallHit.normal;
        Vector3 applyForce = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        playerBody.velocity = new Vector3(playerBody.velocity.x, 0f, playerBody.velocity.z); // reset y velocity, makes jumping smoother
        playerBody.AddForce(applyForce, ForceMode.Impulse); // add jump force
    }
}
