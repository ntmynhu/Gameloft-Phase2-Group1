using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Skill : ScriptableObject
{
    public new string name;
    public int id;
    public Sprite image;

    public InputActionReference action;
    public InputActionReference startAction;
    public InputActionReference endAction;

    public bool isCasted = false;
    public int dmg;
    public float castTime;

    public enum SkillState
    {
        disabled, ready, casting, active
    }

    public SkillState state = SkillState.ready;
    /*  __________________________________
        |           |                    |
        v           V                    |
     disabled <-> ready <-> casting -> active
         ^                     |
         |_____________________| 
    */
                                                        
    public void SetReady()
    {
        state = SkillState.ready;
    }

    public void SetDisabled()
    {
        state = SkillState.disabled;
    }

    public void SetCasting()
    {
        if (state == SkillState.ready)
            state = SkillState.casting;
    }

    public void SetActive()
    {
        if (state == SkillState.casting)
            state = SkillState.active;
    }

    public SkillState GetState()
    {
        return state;
    }

    public virtual void UpdateAimSprite(AimRenderer aimRenderer) { }
    public abstract void Cast(GameObject player = null);
    public virtual void CancelCast(GameObject player = null)
    {

    }    
    public abstract void Activate(GameObject player = null);

    public abstract void PerformCurrentAction(GameObject player = null, AimRenderer aimRenderer = null);
    public abstract void NextStage(string actionName, string actionState);
}
