using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnim;

    private string IS_WALKING = "isWalking";
    private string IS_ATTACKING = "isAttacking";

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
    }
}
