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

    private string HEALTH = "health";
    private string SPEED = "speed";
    private string ATTACK = "attack";
    private string IS_MOVING = "isMoving";

    private float playerSpeed;
    private float health;

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
        health = playerHealth.GetHealth();
        playerAnim.SetFloat(HEALTH, health);

        UpdatePlayerSize();
        HandlePlayerFacing();
        HandleMovingAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            playerAnim.SetTrigger(ATTACK);
        }
    }

    private float smoothedSpeed;
    private float smoothingFactor = 10f;
    private bool isMoving;
    public float threshold = 0.1f;

    private void HandleMovingAnimation()
    {
        float targetSpeed = rb.velocity.magnitude;

        //Debug.Log("target: " + targetSpeed);
        if (Mathf.Abs(targetSpeed - smoothedSpeed) > threshold)
        {
            smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, Time.deltaTime * smoothingFactor);
            //Debug.Log("smooth: " + smoothedSpeed);
        }

        isMoving = smoothedSpeed > threshold;
        playerAnim.SetBool(IS_MOVING, isMoving);
    }

    private void HandlePlayerFacing()
    {
        float horizontalVelocity = rb.velocity.x;

        if (horizontalVelocity > 0.1f) //Facing right
        {
            Debug.Log("Facing right");
            transform.localScale = new Vector3(-1, 1, 1); 
        }
        else if (horizontalVelocity < -0.1f) //Facing left
        {
            Debug.Log("Facing left");
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Change the spriteHolder and the collider of player
    private void UpdatePlayerSize()
    {
        if (health > 6)
        {
            SwapCollider(maxSizeCollider);
        }
        else if (health > 1)
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
