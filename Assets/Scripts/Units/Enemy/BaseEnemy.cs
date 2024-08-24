using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy :MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;
    public Animator animator;
    enum EnemyState
    {
        Chasing,
        Attacking
    }
    private EnemyState CurrentState;
    [SerializeField]
    Rigidbody2D myRB;
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    public virtual void Attack(Transform target) {
        agent.SetDestination(target.position);
        animator.SetTrigger("attack");
    }
    public virtual void Move(Vector2 direction)
    {
        // should remove this func
    }
    public virtual void Move(Transform target)
    {
        animator.SetBool("isMoving", true);
        agent.SetDestination(target.position);
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Update()
    {
       switch (CurrentState)
       {
           case (EnemyState.Chasing):
                Move(Player);
               break;
            case (EnemyState.Attacking):
                Attack(Player);
               break;
       }
       if (IsPlayerInRange()) CurrentState = EnemyState.Attacking;
       else CurrentState = EnemyState.Chasing;

    }
    private bool IsPlayerInRange()
    {
        return (Vector2.Distance(Player.position, transform.position) < attackRange);
    }
    [SerializeField] TakeDamagePublisherSO TakeDamage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       /* if (collision != null)
        {
            TakeDamage.RaiseEvent(attackDamage, this.gameObject.tag, collision.gameObject.GetInstanceID());
        }    */
        
    }
}
