using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private GameObject playerMaxSize;
    [SerializeField] private GameObject playerMidSize;
    [SerializeField] private GameObject playerMinSize;
    [SerializeField] private PlayerHealth playerHealth;

    private Animator playerAnim;
    private Rigidbody2D rb;
    private PolygonCollider2D currentCollider;
    private GameObject spriteHolder;

    private string IS_WALKING = "isWalking";
    private string IS_ATTACKING = "isAttacking";
    private string SIZE = "size";
    private string SPEED = "speed";

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentCollider = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        UpdatePlayerSize();
    }

    private void Update()
    {
        float speed = rb.velocity.magnitude;
        playerAnim.SetFloat(SPEED, speed);
        UpdatePlayerSize();
        HandlePlayerFacing();
    }

    private void HandlePlayerFacing()
    {
        float horizontalVelocity = rb.velocity.x;

        if (horizontalVelocity > 0.1f) //Facing right
        {
            Debug.Log("Facing right");
            spriteHolder.transform.localScale = new Vector3(-1, 1, 1); 
        }
        else if (horizontalVelocity < -0.1f) //Facing left
        {
            Debug.Log("Facing left");
            spriteHolder.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {

        }
    }

    private void UpdatePlayerSize()
    {
        if (playerHealth.GetHealth() > 6)
        {
            spriteHolder = playerMaxSize;
            SwapCollider(spriteHolder.GetComponent<PolygonCollider2D>());
        }
        else if (playerHealth.GetHealth() > 1)
        {
            spriteHolder = playerMidSize;
            SwapCollider(spriteHolder.GetComponent<PolygonCollider2D>());
        }
        else
        {
            spriteHolder = playerMinSize;
            SwapCollider(spriteHolder.GetComponent<PolygonCollider2D>());
        }
    }

    private void SwapCollider(PolygonCollider2D newCollider)
    {
        currentCollider.points = newCollider.points;
    }
}
