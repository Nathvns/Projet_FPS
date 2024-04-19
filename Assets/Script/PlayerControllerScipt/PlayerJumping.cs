using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Player))]
public class PlayerJumping : MonoBehaviour
{  
[SerializeField] float jumpSpeed = 5f;
[SerializeField] float jumpPressBufferTime = 0.05f;
[SerializeField] float JumpGroundGraceTime = 0.2f;



Player player;
PlayerInput playerInput;

bool tryingToJump;
float lastJumpPressTime;
float lastGroundedTime;
InputAction jumpAction;


void Awake()
{
    player = GetComponent<Player>();
    playerInput = GetComponent<PlayerInput>();
    jumpAction = playerInput.actions["jump"];
}

void OnEnable()
{
    player.OnBeforeMove += OnBeforeMove;
    player.OnGroundStateChange += OnGroundStateChange;    
}
void OnDisable()
{
    player.OnBeforeMove -= OnBeforeMove;
    player.OnGroundStateChange -= OnGroundStateChange;
}
/*void Onjump()
{
    tryingToJump = true;
    lastJumpPressTime = Time.time;
}*/
void OnBeforeMove()
{
    var jumpInput = jumpAction.ReadValue<float>();
    if (jumpInput == 0) return;
    lastJumpPressTime = Time.time;

    bool wasTryingToJump = Time.time - lastJumpPressTime < jumpPressBufferTime;
    bool wasGrounded = Time.time - lastGroundedTime < JumpGroundGraceTime;

    bool isOrWasTryingToJump = tryingToJump || (wasTryingToJump && player.IsGrounded);
    bool isOrWasGrounded = player.IsGrounded || wasGrounded;


    if (isOrWasTryingToJump && isOrWasGrounded)
    {
        player.velocity.y += jumpSpeed;
    }
    tryingToJump = false;
}

void OnGroundStateChange(bool isGrounded)
{
    if (!isGrounded) lastGroundedTime = Time.time;
}
}
