using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    //properties
    [SerializeField] private float initSpeed;
    [SerializeField, Range(0.1f, 10f)] private float fixedDecelerationSpeed;
    [SerializeField, Range(0f, 1f)] private float collidedDecelerationSpeed; // current speed will be subtracted when collided with enemies or walls
    [SerializeField] private float stopSpeed;
    [SerializeField] private int damage;

    //Components
    private Collider2D col;
    private Rigidbody2D rb;


    //Updating val
    [SerializeField] private bool isCollectable; // this will be true when the slime bullet stop moving and can be collected
    [SerializeField] private Vector2 direction;
    private Vector2 lastVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = rb.GetComponent<Collider2D>();
        int layerIndex = LayerMask.NameToLayer("Bullet");
        this.gameObject.layer = layerIndex;
    }

    public void SetTag(string tag)
    {
        gameObject.tag = tag;
    }    
    public bool IsCollectable
    {
        get { return isCollectable; }
    }
    private void Update()
    {
        //While bullet is moving, its speed decrease by fixedDecelerationSpeed, once it stops, it becomes Collectable by the player
        if (!isCollectable)
        {
            if (rb.velocity.magnitude > stopSpeed)
            {
                // velocity -> 0
                rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, fixedDecelerationSpeed * Time.deltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
                isCollectable = true;
                int layerIndex = LayerMask.NameToLayer("Bullet_NoDamage"); //Move to another layer to not colliding the enemy
                this.gameObject.layer = layerIndex;
            }
        }
    }

    private void FixedUpdate()
    {
        lastVel = rb.velocity;
    }

    private void OnDisable()
    {
        isCollectable = false;
        int layerIndex = LayerMask.NameToLayer("Bullet");
        this.gameObject.layer = layerIndex;
    }

    public void Shoot(Vector2 dir)
    {
        direction = dir.normalized;
        rb.velocity = direction * initSpeed;
    }

    // Decrease speed if collide with enemies or wall
    public void DecreaseSpeedOnCollision()
    {
        rb.velocity = direction * Mathf.Lerp(lastVel.magnitude, 0f, collidedDecelerationSpeed);
        Debug.Log("Collided! V: " + lastVel.magnitude + "->" + rb.velocity.magnitude);
    }

    //Change direction
    public void ReflectDirection(Vector2 inNormal)
    {
        direction = Vector2.Reflect(direction, inNormal).normalized;
    }

    [SerializeField] private TakeDamagePublisherSO takeDamageSO;
    [SerializeField] private BulletPublisherSO absorbBulletSO;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (!isCollectable)
        {   //collides with an object that doesn't have the same tag and isn't of the same class.
            if (!(collision.gameObject.tag == gameObject.tag) && !(collision.gameObject.GetType() == this.GetType()))
            {
                // bullet reflects and decreases speed
                ReflectDirection(collision.GetContact(0).normal);
                DecreaseSpeedOnCollision();

                //bullet deal damage
                takeDamageSO.RaiseEvent(damage, this.gameObject.tag, collision.gameObject.name);
            } 
        }


        if (collision.gameObject.tag == gameObject.tag)
        {
            //absorbed if that object is a player and bullet is player's bullet
            absorbBulletSO.RaiseEvent(this, this.gameObject.tag);
        }    
           
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

}
