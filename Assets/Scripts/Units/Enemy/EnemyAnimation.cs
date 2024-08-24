using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private GameObject player;
    private Animator enemyAnim;
    private Rigidbody2D rb;

    private string ATTACK = "attack";
    private string IS_MOVING = "isMoving";

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("PlayerContainer");
        enemyAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandlePlayerFacing();
        HandleMovingAnimation();
    }

    private void HandlePlayerFacing()
    {
        float directionX = player.transform.position.x - transform.position.x;

        if (directionX > 0f) //Facing right
        {
            Debug.Log("Facing right");
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (directionX < -0f) //Facing left
        {
            Debug.Log("Facing left");
            transform.localScale = new Vector3(-1, 1, 1);
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
        enemyAnim.SetBool(IS_MOVING, isMoving);
    }
}
