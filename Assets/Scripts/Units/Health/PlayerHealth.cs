using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
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
}
