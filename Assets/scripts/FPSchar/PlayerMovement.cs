using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActions;
    private InputAction moveAction;
    private Vector2 moveInput;

    public CharacterController controller;

    public float speed = 12f;
    void Start()
    {
        
    }
    void Awake()
    {
        moveAction = inputActions.FindActionMap("Player").FindAction("Move");
    }
    private void OnEnable()
    {
        moveAction.Enable();
    }
    void Update()
    {
       moveInput = moveAction.ReadValue<Vector2>();
       Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
       controller.Move(move * speed * Time.deltaTime);
    }
}
