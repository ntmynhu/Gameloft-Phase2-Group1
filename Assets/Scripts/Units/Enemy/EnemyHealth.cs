using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    private void Awake()
    {
        this.gameObject.tag = "Enemy";
    }
    public override void TakeDmg(float dmg)
    {
        base.TakeDmg(dmg);
        if (currentHealth <= 0) Die();
    }
    public float GetHealth()
    {
        return currentHealth;
    }
    public override void Die()
    { 
        gameObject.SetActive(false);
    }
}
