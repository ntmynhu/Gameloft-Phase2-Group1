using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public enum SlimeState { Good, Normal, Bad}
    private SlimeState currentState = SlimeState.Good;
    [SerializeField] private float goodThreshold; // Good State: goodThreshold -> maxHealth
    [SerializeField] private float normalThreshold; // Normal State: normalThresHold -> goodThreshold - 1
    // Bad State: 1 -> normalThreshold - 1

    private void Awake()
    {
        this.gameObject.tag = "Allie";
    }

    public void Absorb(Bullet bullet)
    {
        BulletManager.Instance.bulletPool.Release(bullet);
        //bullet.gameObject.SetActive(false);
        Heal(1);
    }

    public void OnAbsorb(Bullet bullet, string tag)
    {
        if (tag == this.gameObject.tag)
        {
            Absorb(bullet);
        }
    }

    public override void TakeDmg(float dmg)
    {
        base.TakeDmg(dmg);
        if (currentHealth >= goodThreshold)
        {
            ChangeState(SlimeState.Good);
        }
        else if (currentHealth >= normalThreshold)
        {
            ChangeState(SlimeState.Normal);
        }
        else
        {
            ChangeState(SlimeState.Good);
        }
    }

    public void ChangeState(SlimeState state)
    {
        if (currentState == state) return;
        if (state == SlimeState.Good)
        {
            // process good state changes
        }
        else if (state == SlimeState.Normal)
        {
            // process normal state changes
        }
        else
        {
            // process bad state changes
        }
    }
}
