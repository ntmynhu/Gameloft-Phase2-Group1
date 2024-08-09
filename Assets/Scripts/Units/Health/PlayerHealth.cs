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
            ChangeState(SlimeState.Bad);
        }
    }

    public void ChangeState(SlimeState state)
    {
        if (currentState == state) return;
        if (state == SlimeState.Good)
        {
            // process good state changes
            //PlayerSkillManager.Instance.DisableSkill(PlayerSkillManager.Instance.skill_1);
            PlayerSkillManager.Instance.DisableSkill(PlayerSkillManager.Instance.skill_2);
        }
        else if (state == SlimeState.Normal)
        {
            // process normal state changes
            //PlayerSkillManager.Instance.ReadySkill(PlayerSkillManager.Instance.skill_1);
        }
        else
        {
            // process bad state changes
            //PlayerSkillManager.Instance.DisableSkill(PlayerSkillManager.Instance.skill_1);
            PlayerSkillManager.Instance.ReadySkill(PlayerSkillManager.Instance.skill_2);
        }
    }
}
