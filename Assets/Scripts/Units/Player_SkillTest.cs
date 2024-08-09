using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_SkillTest : MonoBehaviour
{
    public SkillActivator_SkillTest activator;
    //public Skill skill_1;
    public Skill skill_2;
    //public int Skill_1_Unlock = 6;
    public int Skill_2_Unlock = 2;

    public float offset = 1f; // shoot offset
    public float maxHealth;
    public float currentHealth;

    private PlayerInput playerInput;

    private void Awake()
    {
        this.gameObject.tag = "Allie";
        currentHealth = maxHealth;

        playerInput = GetComponent<PlayerInput>();
    }

    private void OnSummonSkill()
    {
        Debug.Log("Summon");
        activator.SetSkill(skill_2);
        
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

            //Decrease health by 1 after shooting
            TakeDmg(1);

            Debug.Log("Shooting");
        }
    }

    public void TakeDmg(float health)
    {
        currentHealth -= health;
        CheckSkill();
    }
    public void Heal(float health)
    {
        currentHealth += health;
        CheckSkill();
    }

    private void CheckSkill()
    {

        /*if (currentHealth <= Skill_2_Unlock)
        {
            skill_1.SetDisabled();
            skill_2.SetReady();
        }
        else if (currentHealth <= Skill_1_Unlock)
        {
            skill_1.SetReady();
        }
        else
        {
            skill_1.SetDisabled();
            skill_2.SetDisabled();
        }*/

        if (currentHealth <= Skill_2_Unlock)
        {
            skill_2.SetReady();
        }
        else
        {
            skill_2.SetDisabled();
        }
    }

    public void Absorb(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        Heal(1);
    }

    public void OnGetDamaged(float damage, string tag, int name)
    {
        if (name == this.gameObject.GetInstanceID() && tag != this.gameObject.tag)
        {
            TakeDmg(damage);
        }
    }

    public void OnAbsorb(Bullet bullet, string tag, int name)
    {
        if (name == this.gameObject.GetInstanceID() && tag == this.gameObject.tag)
        {
            Absorb(bullet);
        }
    }

    public void MoveBulletToPlayer(Bullet bullet, Vector3 targetPosition, float duration)
    {
        StartCoroutine(MoveBullet(bullet, targetPosition, duration));
    }

    private IEnumerator MoveBullet(Bullet bullet, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = bullet.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            bullet.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bullet.transform.position = targetPosition;
        bullet.gameObject.SetActive(false);
    }
}
