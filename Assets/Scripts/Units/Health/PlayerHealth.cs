using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public enum SlimeState { Good, Normal, Bad}
    private SlimeState currentState = SlimeState.Good;
    [SerializeField] private float goodThreshold; // Good State: goodThreshold -> maxHealth
    [SerializeField] private float normalThreshold; // Normal State: normalThresHold -> goodThreshold - 1
                                                    // Bad State: 1 -> normalThreshold - 1

    [SerializeField] private FloatPublisherSO sendGoodThresholdSO;
    [SerializeField] private FloatPublisherSO sendNormalThresholdSO;
    [SerializeField] private FloatPublisherSO sendCurrentHealthSO;
    [SerializeField] private FloatPublisherSO sendMaxHealthSO;

    [SerializeField] private VoidPublisherSO disableSkill1SO;
    [SerializeField] private VoidPublisherSO disableSkill2SO;
    [SerializeField] private VoidPublisherSO enableSkill1SO;
    [SerializeField] private VoidPublisherSO enableSkill2SO;

    private void Awake()
    {
        this.gameObject.tag = "Allie";
        skillManager = GetComponent<PlayerSkillManager>();

        sendGoodThresholdSO.RaiseEvent(goodThreshold);
        sendNormalThresholdSO.RaiseEvent(normalThreshold);
        sendMaxHealthSO.RaiseEvent(maxHealth);

        disableSkill1SO.RaiseEvent();
        disableSkill2SO.RaiseEvent();
    }

    public void Absorb(Bullet bullet)
    {
        BulletManager.Instance.bulletPool.Release(bullet);
        //bullet.gameObject.SetActive(false);
        Heal(1);
        sendCurrentHealthSO.RaiseEvent(currentHealth);
    }

    public void OnAbsorb(GameObject bullet, string tag, int id)
    {
        if (tag == this.gameObject.tag && id == gameObject.GetInstanceID())
        {
            Absorb(bullet.GetComponent<Bullet>());
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
        sendCurrentHealthSO.RaiseEvent(currentHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    PlayerSkillManager skillManager;
    public void ChangeState(SlimeState state)
    {
        if (currentState == state) return;
        if (state == SlimeState.Good)
        {
            // process good state changes
            //PlayerSkillManager.Instance.DisableSkill(PlayerSkillManager.Instance.skill_1);
            skillManager.DisableSkill(skillManager.skill_1);
            disableSkill1SO.RaiseEvent();
            skillManager.DisableSkill(skillManager.skill_2);
            disableSkill2SO.RaiseEvent();
        }
        else if (state == SlimeState.Normal)
        {
            // process normal state changes
            //PlayerSkillManager.Instance.ReadySkill(PlayerSkillManager.Instance.skill_1);
            skillManager.ReadySkill(skillManager.skill_1);
            enableSkill1SO.RaiseEvent();
        }
        else
        {
            // process bad state changes
            //PlayerSkillManager.Instance.DisableSkill(PlayerSkillManager.Instance.skill_1);
            skillManager.ReadySkill(skillManager.skill_2);
            enableSkill2SO.RaiseEvent();
            skillManager.DisableSkill(skillManager.skill_1);
            disableSkill1SO.RaiseEvent();
        }
    }
}
