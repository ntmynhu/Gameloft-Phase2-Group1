using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy :MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    enum EnemyState
    {
        Chasing,
        Attacking
    }
    private EnemyState CurrentState;
    [SerializeField]
    Rigidbody2D myRB;
    [SerializeField]
    private float speed=5f;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int maxHP = 100;
    public virtual void Attack() { }
    public virtual void Move(Vector2 direction) 
    {
        direction.Normalize();
        myRB.velocity = speed * direction;
    }
    public virtual void Move(GameObject target)
    {
        transform.position = Vector2.MoveTowards(transform.position,target.transform.position,speed*Time.deltaTime);
    }
    public virtual void TakeDamage(int ammount)
    {
        currentHP -= ammount;
        if (currentHP <= 0)
        {
            Die();
        }
    }
    private void Start()
    {
        currentHP = maxHP;
    }
    private void Update()
    {
        switch (CurrentState)
        {
            case (EnemyState.Chasing):
                Move(Player);
                break;
            case (EnemyState.Attacking):
                //Attacking
                break;
        }
        if (IsPlayerInRange()) CurrentState = EnemyState.Attacking;
        else CurrentState = EnemyState.Chasing;
        
    }
    private bool IsPlayerInRange()
    {
        return (Vector2.Distance(Player.transform.position, transform.position) < attackRange);
    }
    public void Die() { this.gameObject.SetActive(false); }
}
