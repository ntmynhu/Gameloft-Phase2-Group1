using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillManager : MonoBehaviour
{
    public Skill normal_skill; //static shoot
    public Skill skill_1;
    public Skill skill_2;
    public List<Skill> skillList; //All skills stored here
    public SkillActivator activator;

  
    private void Awake()
    {
        activator = GetComponent<SkillActivator>();
        SetSkills(0, 1); //STATIC

        
    }

    private void Update()
    {
        if (activator.crnSkill == null)
        {
            activator.SetSkill(normal_skill, false);
            this.gameObject.GetComponent<ShootProjectiles>().enabled = true;
        }
    }
    private void Action_started_Skill1(InputAction.CallbackContext obj)
    {
        activator.SetSkill(skill_1);
        this.gameObject.GetComponent<ShootProjectiles>().enabled = false;
    }

    private void Action_started_Skill2(InputAction.CallbackContext obj)
    {
        activator.SetSkill(skill_2);
        this.gameObject.GetComponent<ShootProjectiles>().enabled = false;
    }

    public void SetSkills(int skill1ID, int skill2ID)
    {
        if (skill_1 != null)
        {
            skill_1.startAction.action.started -= Action_started_Skill1;
        }
        if (skill_2 != null)
        {
            skill_2.startAction.action.started -= Action_started_Skill2;
        }
        skill_1 = skillList.Find(skill => skill.id == skill1ID);
        skill_2 = skillList.Find(skill => skill.id == skill2ID);
        if (skill_1 != null)
        {
            skill_1.startAction.action.started += Action_started_Skill1;
        }
        if (skill_2 != null)
        {
            skill_2.startAction.action.started += Action_started_Skill2;
        }
    }    
    public void ReadySkill(Skill skill)
    {
        skill.SetReady();
    }

    public void DisableSkill(Skill skill)
    {
        skill.SetDisabled();
    }
}
