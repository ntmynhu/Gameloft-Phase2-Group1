using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D maxSizeCollider;
    [SerializeField] private PolygonCollider2D midSizeCollider;
    [SerializeField] private PolygonCollider2D minSizeCollider;
    [SerializeField] private PlayerHealth playerHealth;

    private Animator playerAnim;
    private Rigidbody2D rb;
    private PolygonCollider2D currentCollider;

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

        if (horizontalVelocity > 0)
        {
            Debug.Log("Facing right");
            Vector3 scale = transform.localScale;
            scale.x *= 1;
            transform.localScale = scale; //Facing right
        }
        else if (horizontalVelocity < 0)
        {
            Debug.Log("Facing left");
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale; //Facing left
        }
    }

    private void UpdatePlayerSize()
    {
        if (playerHealth.GetHealth() > 6)
        {
            SwapCollider(maxSizeCollider);
        }
        else if (playerHealth.GetHealth() > 1)
        {
            SwapCollider(midSizeCollider);
        }
        else
        {
            SwapCollider(minSizeCollider);
        }
    }

    private void SwapCollider(PolygonCollider2D newCollider)
    {
        currentCollider.points = newCollider.points;
    }
}
