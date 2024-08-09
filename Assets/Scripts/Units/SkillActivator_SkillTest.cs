using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActivator_SkillTest : MonoBehaviour
{
    public Skill crnSkill;
    public AimRenderer aimRenderer;

    private void Awake()
    {
        aimRenderer = GetComponent<AimRenderer>();
    }

    public void SetSkill(Skill skill)
    {
        if (crnSkill != null)
        {
            if (crnSkill.GetState() == Skill.SkillState.casting || crnSkill.GetState() == Skill.SkillState.active)
            {
                return;
            }
        }
        crnSkill = skill;
    }

    private void Update()
    {
        if (crnSkill != null)
        {
            // Check if the action is assigned and the action is triggered
            /*if (crnSkill.action != null && crnSkill.action.action.triggered)
            {
                
            }*/

            Debug.Log("Activator" + crnSkill + crnSkill.GetState());
            switch (crnSkill.GetState())
            {
                case Skill.SkillState.ready:
                    if (crnSkill.action.action.ReadValue<float>() > 0)
                    {
                        crnSkill.Cast(gameObject);
                        crnSkill.SetCasting();
                    }
                    break;

                case Skill.SkillState.casting:
                    crnSkill.Cast(gameObject);
                    // Check if the action was released
                    if (!crnSkill.action.action.ReadValue<float>().Equals(1))
                    {
                        if (!crnSkill.isCasted)
                        {
                            crnSkill.SetReady();
                            aimRenderer.DisableAll();
                        }
                        else
                        {
                            crnSkill.SetActive();
                        }
                    }
                    crnSkill.UpdateAimSprite(aimRenderer);
                    break;

                case Skill.SkillState.active:
                    crnSkill.Activate(gameObject);
                    crnSkill.UpdateAimSprite(aimRenderer);
                    break;

                case Skill.SkillState.disabled:
                    aimRenderer.DisableAll();
                    break;
            }
        }
    }
}
