using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//pseudo
public class Enemy_BulletTest : BaseCharacter_BulletTest
{
    private void Awake()
    {
        this.gameObject.tag = "Enemy";
        int layerIndex = LayerMask.NameToLayer("Enemy");
        this.gameObject.layer = layerIndex;
    }

    public void OnGetDamaged(float damage, string tag, int name)
    {
        if (name == this.gameObject.GetInstanceID() && tag != this.gameObject.tag)
        {
            TakeDmg(damage);
        }
    }

    public override void TakeDmg(float health)
    {
        base.TakeDmg(health);
    }
    public override void Heal(float health)
    {
        base.Heal(health);
    }
}

