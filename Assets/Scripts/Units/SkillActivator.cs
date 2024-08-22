using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillActivator : MonoBehaviour
{
    public Skill crnSkill;
    public AimRenderer aimRenderer;
    public bool swapOut = true;
    private void Awake()
    {
        aimRenderer = GetComponent<AimRenderer>();
    }

    public void SetSkill(Skill skill, bool swapOut = true)
    {
        if (crnSkill != null)
        {
            if (crnSkill.GetState() == Skill.SkillState.casting || crnSkill.GetState() == Skill.SkillState.active)
            {
                return;
            }
            UnSubscribeActionEvents(crnSkill);
        }
        crnSkill = skill;
        this.swapOut = swapOut;
        SubscribeActionEvents(crnSkill);
        Action_started();
        Debug.Log("Switched Skill!");

    }

    private void UnSubscribeActionEvents(Skill skill)
    {
        skill.startAction.action.started -= Action_started;
        Debug.Log("UnSubscribing started action..." + skill.startAction.action.name);
        skill.endAction.action.canceled -= Action_canceled;
        Debug.Log("UnSubscribing canceled action..." + skill.endAction.action.name);
        skill.endAction.action.performed -= Action_performed;
        Debug.Log("UnSubscribing performed action..." + skill.endAction.action.name);
    }

    private void SubscribeActionEvents(Skill skill)
    {
        crnSkill.startAction.action.started += Action_started;
        Debug.Log("Subscribing started action..." + skill.startAction.action.name);
        crnSkill.endAction.action.canceled += Action_canceled;
        Debug.Log("Subscribing canceled action..." + skill.endAction.action.name);
        crnSkill.endAction.action.performed += Action_performed;
        Debug.Log("Subscribing performed action..." + skill.endAction.action.name);
    }

    public enum ActivatorState
    {
        started, canceled, performed, waiting
    }
    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (crnSkill != null)
        {
            crnSkill.NextStage(obj.action.name, "Perform");
        }
        Debug.Log("Action performed..." + obj.action.name);
    }

    private void Action_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        if (crnSkill != null)
        {
            crnSkill.NextStage(obj.action.name, "Start");
        }
        Debug.Log("Action started..."+obj.action.name);
    }

    private void Action_started()
    {

        if (crnSkill != null)
        {
            crnSkill.NextStage(crnSkill.startAction.action.name, "Start");
        }
        Debug.Log("Action started..." + crnSkill.startAction.action.name);
    }

    private void Action_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (crnSkill!=null)
        {
            crnSkill.NextStage(obj.action.name, "Cancel");
        }
        Debug.Log("Action canceled..." + obj.action.name);
    }
    private void Update()
    {
        if (crnSkill != null)
        {
            crnSkill.PerformCurrentAction(gameObject, aimRenderer);
            switch (crnSkill.GetState())
            {
                case Skill.SkillState.ready:
                    if (swapOut)
                    {
                        UnSubscribeActionEvents(crnSkill);
                        aimRenderer.DisableAll();
                        crnSkill = null;
                    }
                    break;
                case Skill.SkillState.disabled:
                    UnSubscribeActionEvents(crnSkill);
                    aimRenderer.DisableAll();
                    crnSkill = null;
                    break;
            }
        }
    }
}
