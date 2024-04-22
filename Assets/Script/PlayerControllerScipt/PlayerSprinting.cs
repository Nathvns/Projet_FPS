
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerSprinting : MonoBehaviour
{
[SerializeField] float speedMultiplier = 2f;
Player player;
PlayerInput playerInput;
InputAction sprintAction;
void Awake()
{
    player = GetComponent<Player>();
    playerInput = GetComponent<PlayerInput>();
    sprintAction = playerInput.actions["sprint"];
}

void OnEnable() => player.OnBeforeMove += OnBeforeMove;
void OnDisable() => player.OnBeforeMove -= OnBeforeMove;


void OnBeforeMove() {
    float sprintInput = sprintAction.ReadValue<float>();
    if (sprintInput == 0) return;
    float forwardMovementFactor = Mathf.Clamp01(
        Vector3.Dot(player.transform.forward, player.velocity.normalized)
    );
    float multiplier = Mathf.Lerp(1f, speedMultiplier, forwardMovementFactor);
    player.movementSpeedMultiplier *= multiplier;
}

}