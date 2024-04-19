
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;



public class Player : MonoBehaviour
{
   [SerializeField] Transform cameraTransform;
   [SerializeField] float movementSpeed = 5f;

   [SerializeField] float mass = 1f;
   [SerializeField] float acceleration = 20f;
   [SerializeField] float mouseSensitivity = 3f;
   [SerializeField] private Interactable interactableTarget;
    public bool IsGrounded => controller.isGrounded;
    public event Action OnBeforeMove;
    public event Action<bool> OnGroundStateChange;
   internal float movementSpeedMultiplier;
    public CharacterController controller;
   internal Vector3 velocity;
   Vector2 look;
   bool wasGrounded;

   PlayerInput playerInput;
   InputAction moveAction;
   InputAction lookAction;
   InputAction sprintAction;
   InputAction useAction;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];
        sprintAction = playerInput.actions["sprint"];
        useAction = playerInput.actions["use"];



    }
    void Update()
    {
       UpdateGround();
       UpdateMovement();
       UpdateLook();
       UpdateGravity();
       InteractionHandler();
       UseInputHandler();
    }

    void UpdateGround()
    {
        if (wasGrounded != IsGrounded)
        {
            OnGroundStateChange?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }
    }
    Vector3 GetMovementInput()
    {
        var moveInput = moveAction.ReadValue<Vector2>();

        var input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        var sprintInput = sprintAction.ReadValue<float>();
        var multiplier = sprintInput > 0 ? 1.5f : 1f;
        input *= movementSpeed * movementSpeedMultiplier;
        return input;
    }
    void UpdateMovement(){
        movementSpeedMultiplier = 1f;
        OnBeforeMove?.Invoke();
        var input = GetMovementInput();
      
        var factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        //transform.Translate(input * movementSpeed * Time.deltaTime, Space.World);
        controller.Move(velocity * Time.deltaTime);
    }
    void UpdateLook(){
        var lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensitivity;
        look.y += lookInput.y * mouseSensitivity;

        look.y = Mathf.Clamp(look.y, -89f, 89f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);        
    }
void UpdateGravity(){
    var gravity = Physics.gravity * mass * Time.deltaTime;
    velocity.y = controller.isGrounded ? -1f : velocity.y + gravity.y;



}

void InteractionHandler(){
    Vector3 playerOrientation = cameraTransform.forward;

    if(Physics.Raycast(transform.position, playerOrientation, out RaycastHit hitObj, 5f, LayerMask.GetMask("Usables"))){

        

        if(hitObj.transform.TryGetComponent<Interactable>(out Interactable interactableObj)){
            interactableTarget = interactableObj;
        }
    } else {
            interactableTarget = null;
        }
}

void UseInputHandler (){
    var useInput = useAction.ReadValue<float>();
    if (useInput == 0) return;
    Debug.Log("coucou");
    interactableTarget?.Interact();


}

}
