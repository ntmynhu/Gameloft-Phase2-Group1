using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy :MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;
    enum EnemyState
    {
        Chasing,
        Attacking
    }
    private EnemyState CurrentState;
    [SerializeField]
    Rigidbody2D myRB;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float attackRange = 10f;
    [SerializeField]
    private int attackDamage = 1;
    [SerializeField]
    public virtual void Attack() { }
    public virtual void Move(Vector2 direction)
    {
        // should remove this func
    }
    public virtual void Move(Transform target)
    {
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
       // switch (CurrentState)
       // {
           // case (EnemyState.Chasing):
                Move(Player);
               // break;
            //case (EnemyState.Attacking):
                //Attacking
              //  break;
       // }
       // if (IsPlayerInRange()) CurrentState = EnemyState.Attacking;
       // else 
      //  CurrentState = EnemyState.Chasing;

    }
    private bool IsPlayerInRange()
    {
        return (Vector2.Distance(Player.transform.position, transform.position) < attackRange);
    }
    [SerializeField] TakeDamagePublisherSO TakeDamage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage.RaiseEvent(attackDamage, this.gameObject.tag, collision.gameObject.GetInstanceID());
    }
}
