using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public Skill skill_1;
    public Skill skill_2;

    private static PlayerSkillManager instance;
    public static PlayerSkillManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(instance);
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
