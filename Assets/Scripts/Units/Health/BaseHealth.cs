using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected VoidPublisherSO onDie;
    public virtual void TakeDmg(float dmg)
    {
        currentHealth -= dmg;
    }

    public virtual void Sacrifice(float remaining)
    {
        currentHealth = remaining;
    }

    public virtual void OnGetDamaged(float damage, string tag, int id)
    {
        if (id == this.gameObject.GetInstanceID() && tag != this.gameObject.tag)
        {
            TakeDmg(damage);
        }
    }

    public virtual void Heal(float hp)
    {
        currentHealth += hp;
    }

    public void Die()
    {
        onDie.RaiseEvent();
        // Destroy or disable player..
    }
}
