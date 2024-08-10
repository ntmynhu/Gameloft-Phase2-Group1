using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Skill : ScriptableObject
{
    public new string name;
    public int id;
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
        state = SkillState.casting;
    }

    public void SetActive()
    {
        state = SkillState.active;
    }

    public SkillState GetState()
    {
        return state;
    }

    public virtual void UpdateAimSprite(AimRenderer aimRenderer) { }
    public abstract void Cast(GameObject player = null);
    public abstract void Activate(GameObject player = null);
}
