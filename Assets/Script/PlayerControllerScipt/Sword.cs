using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    [SerializeField] private Animator swordAnimator;

    public void Running_Sword(bool isRunning)
    {
        swordAnimator.SetBool("is_Running", isRunning);
    }

    public void Walking_Sword(bool isWalking)
    {
        swordAnimator.SetBool("is_Walking", isWalking);
    }

    public void Attack_Sword(bool isAttacking)
    {
        swordAnimator.SetBool("attack", isAttacking);
    }
}