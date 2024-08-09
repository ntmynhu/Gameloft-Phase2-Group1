using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillActivator : MonoBehaviour
{
    public Skill crnSkill;
    public AimRenderer aimRenderer;
    [SerializeField] ActivatorState activatorState;
    private void Awake()
    {
        aimRenderer = GetComponent<AimRenderer>();
    }

    public void SetSkill(Skill skill, bool isStarted = true)
    {
        if (crnSkill != null)
        {
            if (crnSkill.GetState() == Skill.SkillState.casting || crnSkill.GetState() == Skill.SkillState.active)
            {
                return;
            }
            UnSubscribeActionEvents(crnSkill);
            Debug.Log("Switched Skill!");
        }
        crnSkill = skill;

        SubscribeActionEvents(crnSkill);
        Debug.Log("Switched Skill!");
        if (isStarted)
            activatorState = ActivatorState.started;
        else
            activatorState = ActivatorState.waiting;

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
        if (activatorState == ActivatorState.started)
        {
            activatorState = ActivatorState.performed;
            Debug.Log("Action performed..." + obj.action.name);
        }
    }

    private void Action_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        activatorState = ActivatorState.started;
        Debug.Log("Action started..."+obj.action.name);
    }
    private void Action_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activatorState == ActivatorState.started)
        {
            activatorState = ActivatorState.canceled;
            Debug.Log("Action canceled..." + obj.action.name);
        }
    }
    private void Update()
    {
        if (crnSkill != null)
        {
            // Check if the action is assigned and the action is triggered
            /*if (crnSkill.action != null && crnSkill.action.action.triggered)
            {
                
            }*/

            //Debug.Log("Activator " + crnSkill + crnSkill.GetState());
            switch (crnSkill.GetState())
            {
                case Skill.SkillState.ready:
                    if (activatorState==ActivatorState.started)
                    {
                        crnSkill.Cast(gameObject);
                        crnSkill.SetCasting();
                    }
                    break;

                case Skill.SkillState.casting:
                    crnSkill.Cast(gameObject);
                    // Check if the action was released
                    crnSkill.UpdateAimSprite(aimRenderer);
                    if (activatorState == ActivatorState.performed)
                    {
                        crnSkill.SetActive();
                        Debug.Log("lmao");
                    }
                    else if (activatorState == ActivatorState.canceled)
                    {
                        crnSkill.SetReady();
                        aimRenderer.DisableAll();
                        UnSubscribeActionEvents(crnSkill);
                        crnSkill = null;
                    }
                    
                    break;

                case Skill.SkillState.active:
                    crnSkill.Activate(gameObject);
                    crnSkill.UpdateAimSprite(aimRenderer);
                    activatorState = ActivatorState.waiting;
                    break;

                case Skill.SkillState.disabled:
                    aimRenderer.DisableAll();
                    UnSubscribeActionEvents(crnSkill);
                    crnSkill = null;
                    break;
            }
        }
    }
}
