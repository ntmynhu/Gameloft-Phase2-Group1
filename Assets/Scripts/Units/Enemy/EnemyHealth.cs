using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] SpriteRenderer bloodSprite;
    private void Awake()
    {
        this.gameObject.tag = "Enemy";
    }
    public override void TakeDmg(float dmg)
    {
        base.TakeDmg(dmg);
        bloodSprite.size = new Vector2((float)currentHealth/(float)maxHealth, bloodSprite.size.y);
        if (currentHealth <= 0) Die();
    }
    public float GetHealth()
    {
        return currentHealth;
    }

    public override void Die()
    {
        base.Die();
        currentHealth = maxHealth;
        bloodSprite.size = new Vector2((float)currentHealth / (float)maxHealth, bloodSprite.size.y);
    }
}
