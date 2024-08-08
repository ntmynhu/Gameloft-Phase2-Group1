using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacter_BulletTest : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public virtual void TakeDmg(float dmg)
    {
        currentHealth -= dmg;
    }
    public virtual void Heal(float hp)
    {
        currentHealth += hp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}