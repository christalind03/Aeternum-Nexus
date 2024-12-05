using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Dashing : MonoBehaviour
{


    [Header("References")]
    public Transform orientation;
    public Transform playerCamera;
    public InputActionAsset playerControls;
    Rigidbody playerBody;
    PlayerMovement playerMovement;
    PlayerAudio playerAudio;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCoolDown;
    float dashCoolDownTimer;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVelocity = true;

    [Header("Effects")]
    public PlayerCamera cameraEffect;
    public float dashFov;

    [Header("Input")]
    InputAction dashAction; // E by default cuz y'all are weird
    InputAction moveAction;
    Vector2 moveInput;

    [Header("Dash Bar")]
    [SerializeField] private Image _DashBarFIll;
    [SerializeField] private Image _DashBarBack;


    // Start is called before the first frame update
    void Start()
    {
        moveAction = playerControls.FindActionMap("Movement").FindAction("Move");
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        dashAction = playerControls.FindActionMap("Movement").FindAction("Dash");

        playerBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAudio = GetComponent<PlayerAudio>();
    }   

    // Update is called once per frame
    void Update()
    {
        if (dashAction.WasPressedThisFrame())
        {
            Dash();
            UpdateDashBar();
        }
        if (dashCoolDownTimer > 0)
        {
            dashCoolDownTimer -= Time.deltaTime;
            UpdateDashBar();
        }
    }

    void Dash()
    {
        if (dashCoolDownTimer > 0 || playerMovement.state == PlayerMovement.MovementState.wallrunning) // dash cool down is still active, ignore function // also don't allow dashing while wallrunning lmao
        {
            return;
        }
        
        playerAudio.PlayPlayerAudio("dash");
        dashCoolDownTimer = dashCoolDown;

        playerMovement.isDashing = true;
        playerMovement.maxYSpeed = maxDashYSpeed;

        cameraEffect.DoFov(dashFov); // cameraEffect.DoFov(fieldOfView + 15);

        Transform forwardTransform;
        if (useCameraForward)
        {
            forwardTransform = playerCamera;
        }
        else
        {
            forwardTransform = orientation;
        }

        Vector3 direction = GetDirection(forwardTransform);
        Vector3 applyForce = direction * dashForce + orientation.up * dashUpwardForce;

        if (disableGravity)
        {
            playerBody.useGravity = false;
        }

        delayedApplyForce = applyForce;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration); // stops dash
    }
    private void UpdateDashBar()
    {
        if (gameObject.CompareTag("Player"))
        {
            float fillAmount = dashCoolDownTimer;
            _DashBarFIll.fillAmount = fillAmount;
        }
        
    }

    Vector3 delayedApplyForce;
    void DelayedDashForce()
    {
        if (resetVelocity)
        {
            playerBody.velocity = Vector3.zero;
        }
        playerBody.AddForce(delayedApplyForce, ForceMode.Impulse);
    }
    
    void ResetDash()
    {
        playerMovement.isDashing = false;
        playerMovement.maxYSpeed = 0;
        cameraEffect.DoFov(80f); // cameraEffect.DoFov(fieldOfView);
        
        if (disableGravity)
        {
            playerBody.useGravity = true;
        }
    }

    Vector3 GetDirection(Transform forwardTransform)
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;

        Vector3 direction = new Vector3();

        if (allowAllDirections) // allows dashing in all directions
        {
            direction = forwardTransform.forward * verticalInput + forwardTransform.right * horizontalInput;
        }
        else
        {
            direction = forwardTransform.forward;
        }

        if (verticalInput == 0 && horizontalInput == 0) // dash forward by default if no direction is given
        {
            direction = forwardTransform.forward;
        }
        return direction.normalized;
    }
}
