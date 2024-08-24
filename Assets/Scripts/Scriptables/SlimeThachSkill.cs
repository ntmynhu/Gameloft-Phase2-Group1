using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SlimeThachSkill")]

public class SlimeThachSkill : Skill
{
    //Press X to cast, IJKL to move the aimer
    public float radius = 0.5f;
    public float radiusMultipler = 0.5f;
    public Vector3 aimPos;
    public float aimSpeed;
    public Sprite skillAimSprite;
    public int remainingHealth;

    //Update the SkillAimVisualize
    public override void UpdateAimSprite(AimRenderer aimRenderer)
    {
        aimRenderer.AimSpriteRenderer.enabled = true;
        // Set the position of the sprite to the caster position
        aimRenderer.AimSpriteRenderer.sprite = skillAimSprite;
        aimRenderer.AimSpriteRenderer.transform.position = aimPos;

        // Scale the sprite based on the desired radius
        float scale = radius * 2.0f / aimRenderer.AimSpriteRenderer.sprite.bounds.size.x;
        aimRenderer.AimSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
    }


    [SerializeField] private TakeDamagePublisherSO takeDamageSO;
    public override void Activate(GameObject Caster)
    {
        //Use CircleCast to know if any object is within the skill range
        RaycastHit2D[] hit;

        Vector3 p1 = aimPos;

        hit = Physics2D.CircleCastAll(p1, radius, Vector2.right, 1);
        if (hit.Length > 0)
        {
            Debug.Log("Hit objects: " + hit.Length);
            for (int i = 0; i < hit.Length; i++)
            {
                Debug.Log("Hit: " + hit[i].collider.name);

                //Change enemy's health
                if (!(hit[i].collider.gameObject.tag == Caster.tag))
                {
                    takeDamageSO.RaiseEvent(dmg, Caster.tag, hit[i].collider.gameObject.GetInstanceID());
                }
            }

        }

        //Change Character Health
        takeDamageSO.RaiseEvent(crnHealth-remainingHealth, "Slime Thach", Caster.gameObject.GetInstanceID());
        BulletManager.Instance.GetBulletToTransform(aimPos, radius, Caster.tag, (int)crnHealth - remainingHealth);
        SetDisabled(); //Disable the skill on complete
    }

    public float crnHealth;
    public override void Cast(GameObject Caster)
    {
        //Moving the aim Circle
        
        if (isCasted == true)
        {
            
            crnHealth = Caster.GetComponent<PlayerHealth>().GetHealth();
            radius = (crnHealth - remainingHealth) * radiusMultipler;
            isCasted = false;

        }
        aimPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

    }

    public override void NextStage(string actionName, string actionState)
    {
        if (actionName == startAction.action.name)
        {
            switch (actionState)
            {
                case "Start":
                    SetCasting();
                    break;
            }
        }
        else if (actionName == endAction.action.name)
        {
            switch (actionState)
            {
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
                aimRenderer.DisableAll();
                break;
            case SkillState.casting:
                Cast(player);
                UpdateAimSprite(aimRenderer);
                break;
            case SkillState.active:
                Activate(player);
                isCasted = true;
                break;
        }
    }

}

