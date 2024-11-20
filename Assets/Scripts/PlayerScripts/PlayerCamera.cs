using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera")]
    public float sensitivityX;
    public float sensitivityY;
    public float startingPlayerRotation;

    // public fieldOfView;
    
    [Header("References")]
    public Transform orientation;
    public Transform cameraHolder;
    public InputActionAsset playerControls;
    InputAction lookAction;
    Vector2 lookInput;

    float horizontalRotation; // horizontal rotation is on the y axis
    float verticalRotation; // vertical rotation is on the x axis

    // Start is called before the first frame update
    private void Start() // locks cursor to middle of the screen and hides it
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lookAction = playerControls.FindActionMap("Movement").FindAction("Look");
        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;

        horizontalRotation = startingPlayerRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        // get mouse input
        float mouseX = lookInput.x * Time.deltaTime * sensitivityX;
        float mouseY = lookInput.y * Time.deltaTime * sensitivityY;

        horizontalRotation += mouseX;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f); // locks vertical rotation to 90 degrees

        // rotate camera and orientation
        cameraHolder.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        orientation.rotation = Quaternion.Euler(0, horizontalRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}