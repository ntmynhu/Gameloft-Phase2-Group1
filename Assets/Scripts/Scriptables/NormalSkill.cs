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

    }

    public override void UpdateAimSprite(AimRenderer aimRenderer)
    {
        
    }

    [SerializeField] private TakeDamagePublisherSO takeDamageSO;
    public override void Activate(GameObject Caster)
    {

        Debug.Log("NormalSkill Activate");
        takeDamageSO.RaiseEvent(1, "Shoot", Caster.gameObject.GetInstanceID());
        this.SetReady();
    }
}
