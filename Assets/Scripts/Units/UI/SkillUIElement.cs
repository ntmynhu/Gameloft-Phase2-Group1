using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIElement : MonoBehaviour
{
    [SerializeField] private Skill skill;

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        this.gameObject.GetComponent<Image>().sprite = skill.skillImage;
        DisableSkillUI(skill.id);
    }

    public void EnableSkillUI(float id)
    {
        if (id == skill.id)
        {
            Color color = this.gameObject.GetComponent<Image>().color;
            color.a = 1f;
            Debug.Log("ENABLE");
            this.gameObject.GetComponent<Image>().color = color;
        }
    }

    public void DisableSkillUI(float id)
    {
        if (id == skill.id)
        {
            Color color = this.gameObject.GetComponent<Image>().color;
            color.a = 0.5f;
            Debug.Log("DISABLE");
            this.gameObject.GetComponent<Image>().color = color;
        }
    }
}
