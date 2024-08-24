using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;


[CreateAssetMenu(menuName = "NormalSkill")]
public class NormalSkill : Skill
{
    public override void Cast(GameObject Caster)
    {
        if(isCasted== false)
        {
            Debug.Log("NormalSkill Activate");
            SacrificeSO.RaiseEvent(1);
            isCasted = true;
        }
        
        
    }

    public override void UpdateAimSprite(AimRenderer aimRenderer)
    {
        
    }

    [SerializeField] private FloatPublisherSO SacrificeSO;


    public override void Activate(GameObject Caster)
    {
        Debug.Log("NormalSkill Pang Pang");
        this.SetReady();
        isCasted = false;
    }

    public override void NextStage(string actionName, string actionState)
    {
        if(actionName == startAction.action.name)
        {
            switch(actionState)
            {
                case "Start":
                    SetCasting();
                    break;
                case "Cancel":
                case "Perform":
                    SetActive();
                    break;
            }
        }
    }

    public override void PerformCurrentAction(GameObject player = null, AimRenderer aimRenderer = null)
    {
        switch (state)
        {
            case SkillState.ready:
            case SkillState.disabled:
                break;
            case SkillState.casting:
                Cast(player);
                break;
            case SkillState.active:
                Activate(player);
                break;
        }    
    }
}
