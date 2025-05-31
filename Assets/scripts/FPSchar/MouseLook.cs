using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction lookAction;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private float xRotation = 0f;

    public float mouseSensitivity = 100f;
    public Transform playerbody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Awake()
    {
        lookAction = inputActions.FindActionMap("Player").FindAction("Look");
    }
    private void OnEnable()
    {
        lookAction.Enable();
    }
    void Update()
    {
        lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;
        
        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerbody.Rotate(Vector3.up * lookInput.x);
    }
}
