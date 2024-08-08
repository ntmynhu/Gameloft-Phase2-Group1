using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//pseudo
public class Player_BulletTest : BaseCharacter_BulletTest
{
    private void Awake()
    {
        this.gameObject.tag = "Allie";
    }

    public override void TakeDmg(float health)
    {
        base.TakeDmg(health);
    }
    public override void Heal(float health)
    {
        base.Heal(health);
    }
    public void Absorb(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        Heal(1);
    }
    public float offset = 1f; // shoot offset

    public void OnGetDamaged(float damage, string tag, string name)
    {
        if (name == this.gameObject.name && tag != this.gameObject.tag)
        {
            TakeDmg(damage);
        }
    }

    public void OnAbsorb(Bullet bullet, string tag)
    {
        if (tag == this.gameObject.tag)
        {
            Absorb(bullet);
        }
    }
    private void Update()
    {
        //pseudo
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Bullet bullet = BulletManager.Instance.GetBullet(this.gameObject.tag);
            Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            // Apply the offset along the direction
            Vector2 offsetPosition = (Vector2)this.transform.position + direction.normalized * offset;

            // Set the bullet's position
            bullet.transform.position = offsetPosition;
            bullet.Shoot(direction);

            Debug.Log("Shooting");
        }
    }
}

