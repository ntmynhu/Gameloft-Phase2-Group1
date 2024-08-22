using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIElement : MonoBehaviour
{
    [SerializeField] private Skill skill;

    private void Start()
    {
        this.gameObject.GetComponent<Image>().sprite = skill.image;
    }

    public void EnableSkillUI(float id)
    {
        if (id == skill.id)
        {
            Color color = this.gameObject.GetComponent<Image>().material.color;
            color.a = 1f;
            this.gameObject.GetComponent<Image>().material.color = color;
        }
    }

    public void DisableSkillUI(float id)
    {
        if (id == skill.id)
        {
            Color color = this.gameObject.GetComponent<Image>().material.color;
            color.a = 0.5f;
            this.gameObject.GetComponent<Image>().material.color = color;
        }
    }
}
