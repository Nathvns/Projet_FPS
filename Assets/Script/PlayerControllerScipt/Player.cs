using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
   // Définition des variables pour la caméra, la vitesse de déplacement, la masse, l'accélération, la sensibilité de la souris et l'objet interactable
   [SerializeField] Transform cameraTransform;
   [SerializeField] float movementSpeed = 5f;
   [SerializeField] float mass = 1f;
   [SerializeField] float acceleration = 20f;
   [SerializeField] float mouseSensitivity = 3f;
   [SerializeField] private Interactable interactableTarget;
   internal float movementSpeedMultiplier;
   public Sword sword;
   public Slime_Mob slimeMob;

   // Définition des événements pour le mouvement et le changement d'état du sol
   public event Action OnBeforeMove;
   public event Action<bool> OnGroundStateChange;
   public bool IsGrounded => controller.isGrounded;


   // Définition des variables pour le contrôleur de personnage, la vitesse, le regard et l'état du sol
   public CharacterController controller;
   internal Vector3 velocity;
   Vector2 look;
   bool wasGrounded;

   // Définition des actions pour le mouvement, le regard, le sprint et l'utilisation
   PlayerInput playerInput;
   InputAction moveAction;
   InputAction lookAction;
   InputAction sprintAction;
   InputAction useAction;
   InputAction attackAction;


    void Start()
    {
        // Verrouillage du curseur au démarrage
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Awake()
    {
        // Initialisation des actions et du contrôleur de personnage
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["move"];
        lookAction = playerInput.actions["look"];
        sprintAction = playerInput.actions["sprint"];
        useAction = playerInput.actions["use"];
        attackAction = playerInput.actions["attack"];
        
    }
    void Update()
{
    // Mise à jour de l'état du sol, du mouvement, du regard, de la gravité et de l'interaction
    UpdateGround();
    UpdateMovement();
    UpdateLook();
    UpdateGravity();
    InteractionHandler();
    UseInputHandler();

    // Reset all animations
    sword.Running_Sword(false);
    sword.Walking_Sword(false);
    sword.Attack_Sword(false);

    // Check if the player is sprinting
    if (sprintAction.ReadValue<float>() > 0)
    {
        sword.Running_Sword(true);
    }
    // Check if the player is moving
    else if (moveAction.ReadValue<Vector2>().magnitude > 0)
    {
        sword.Walking_Sword(true);
    }

    float attackInput = attackAction.ReadValue<float>();
if (attackInput > 0)
{
    sword.Attack_Sword(true);
}
else
{
    sword.Attack_Sword(false);
}
}



    void UpdateGround()
    {
        // Vérification du changement d'état du sol
        if (wasGrounded != IsGrounded)
        {
            OnGroundStateChange?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }
    }
    Vector3 GetMovementInput()
    {
        // Obtention de l'entrée de mouvement et de sprint
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        Vector3 input = new Vector3();
        input += transform.forward * moveInput.y;
        input += transform.right * moveInput.x;
        input = Vector3.ClampMagnitude(input, 1f);
        float sprintInput = sprintAction.ReadValue<float>();
        float multiplier = sprintInput > 0 ? 1.5f : 1f;
        input *= movementSpeed * movementSpeedMultiplier;
        return input;
    }
    void UpdateMovement(){
        // Mise à jour du mouvement du personnage
        movementSpeedMultiplier = 1f;
        OnBeforeMove?.Invoke();
        Vector3 input = GetMovementInput();
      
        float factor = acceleration * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, input.x, factor);
        velocity.z = Mathf.Lerp(velocity.z, input.z, factor);

        controller.Move(velocity * Time.deltaTime);
    }
    void AttackHandler(){
        bool attackInput = attackAction.ReadValue<bool>();
        sword.Attack_Sword(!attackInput);
        slimeMob.Attacked();}

    void UpdateLook(){
        // Mise à jour du regard du personnage
        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        look.x += lookInput.x * mouseSensitivity;
        look.y += lookInput.y * mouseSensitivity;

        look.y = Mathf.Clamp(look.y, -89f, 89f);

        cameraTransform.localRotation = Quaternion.Euler(-look.y, 0, 0);
        transform.localRotation = Quaternion.Euler(0, look.x, 0);        
    }
void UpdateGravity(){
    // Mise à jour de la gravité du personnage
    Vector3 gravity = Physics.gravity * mass * Time.deltaTime;
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
    // Gestion de l'entrée d'utilisation
    float useInput = useAction.ReadValue<float>();
    if (useInput == 0) return;
    Debug.Log("coucou");
    interactableTarget?.Interact();

}}
