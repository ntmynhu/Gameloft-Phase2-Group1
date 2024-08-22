using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill")]
public class SummonSkill : Skill
{
    public float activeDuration;
    public float countdownTime; // The time in seconds for the countdown
    //public float maxCountdownTime = 3f;
    private List<Vector3> linePositions = new List<Vector3>();
    private List<bool> lineHitted = new List<bool>();
    
    private void AddLinePosition(Vector3 start, Vector3 end, bool hitted = false)
    {
        linePositions.Add(start);
        linePositions.Add(end);
        lineHitted.Add(hitted);
    }
    public override void Cast(GameObject Caster)
    { 
        if (countdownTime <= 0)
        {
            isCasted = true;
            this.SetActive();
            
        }
        else
        {
            countdownTime -= Time.deltaTime;
        }

        linePositions.Clear();
        Bullet[] bullets = FindObjectsOfType<Bullet>();

        bool hitted = false;
        foreach (Bullet bullet in bullets)
        {
            if (bullet != null)
            {
                if (bullet.IsCollectable)
                {
                    // Visualize the line in the Scene view
                    // Perform the Linecast
                    /*RaycastHit2D[] hit = Physics2D.LinecastAll(bullet.transform.position, Caster.transform.position);
                    if (hit.Length > 0)
                    {
                        for (int i = 0; i < hit.Length; i++)
                        {
                            if (hit[i].collider.gameObject.tag == "Enemy")
                            {
                                hitted = true;
                                break;
                            }
                        }
                    }*/

                    AddLinePosition(bullet.transform.position, Caster.transform.position, hitted);
                }
            }
        }
        Caster.GetComponent<PlayerMovement>().enabled = false;
    }
    [SerializeField]
    Texture2D colorsTexture;
    public override void UpdateAimSprite(AimRenderer aimRenderer)
    {
        aimRenderer.AimLineRenderer.enabled = true;

        aimRenderer.AimLineRenderer.positionCount = linePositions.Count;
        for (int i = 0; i < linePositions.Count; i++)
        {
            // Set color for the segment

            aimRenderer.AimLineRenderer.SetPosition(i, linePositions[i]);
        }
    }


    [SerializeField] private TakeDamagePublisherSO takeDamageSO;
    [SerializeField] private GameObjectPublisherSO bulletPublisherSO;

    public override void CancelCast(GameObject Caster)
    {
        Caster.GetComponent<PlayerMovement>().enabled = true;
    }
    public override void Activate(GameObject Caster)
    {

        Debug.Log("Summonskill Activate");

        //Player_SkillTest main = Caster.GetComponent<Player_SkillTest>();

        //Find each bullet and try a Linecast
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        Debug.Log(bullets.Length);
        foreach (Bullet bullet in bullets)
        {
            if (bullet != null)
            {
                if (bullet.IsCollectable && bullet.tag == Caster.tag)
                {
                    Collider2D bulletCollide = bullet.GetComponent<Collider2D>();

                    // Perform the Linecast
                    RaycastHit2D[] hit = Physics2D.LinecastAll(bullet.transform.position, Caster.transform.position);

                    // Check if the line hit something in the first loop
                    if (hit.Length > 0 && bulletCollide.enabled != false)
                    {
                        Debug.Log("Hit objects: " + hit.Length);
                        for (int i = 0; i < hit.Length; i++)
                        {
                            Debug.Log("Hit: " + hit[i].collider.name);
                            //Change enemy's health
                            if (!(hit[i].collider.gameObject.tag == Caster.tag))
                            {
                                // Visualize the line in the Scene view
                                Debug.Log("Deal Damage On: " + hit[i].collider.name);
                                takeDamageSO.RaiseEvent(dmg, Caster.tag, hit[i].collider.gameObject.GetInstanceID());
                            }
                        }
                    }
                    
                    bulletCollide.enabled = false; //Turn off the collision of the bullet, as we use raycast instead 

                    //Move the bullet back to Caster (doTween)
                    bullet.transform.DOMove(Caster.transform.position, activeDuration)
                        .SetEase(Ease.Linear) // Set movement to linear (no acceleration/deceleration)
                        .OnComplete(() => OnMovementComplete(bullet, Caster));

                    //main.MoveBulletToPlayer(bullet, main.gameObject.transform.position, activeDuration);
                    
                    //main.MoveBulletToPlayer(bullet, main.gameObject.transform.position, activeDuration);
                }

            }
        }
    }

    private void OnMovementComplete(Bullet bullet, GameObject Caster)
    {   if (bullet.gameObject.activeSelf)
        {
            bulletPublisherSO.RaiseEvent(bullet.gameObject, Caster.tag, Caster.gameObject.GetInstanceID());
        }
        linePositions.Clear();
        Caster.GetComponent<PlayerMovement>().enabled = true;
        SetDisabled();
    }

    public override void NextStage(string actionName, string actionState)
    {
        if (actionName == startAction.action.name)
        {
            switch (actionState)
            {
                case "Start":
                    SetCasting();
                    countdownTime = castTime;
                    break;
                case "Cancel":
                    if(isCasted == false)
                        SetReady();
                    break;
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
                if (isCasted == false)
                {
                    CancelCast(player);
                    aimRenderer.DisableAll();
                    isCasted = true;
                }
                break;
            case SkillState.disabled:
                aimRenderer.DisableAll();
                break;
            case SkillState.casting:
                isCasted = false;
                Cast(player);
                UpdateAimSprite(aimRenderer);
                break;
            case SkillState.active:
                Activate(player);
                UpdateAimSprite(aimRenderer);
                break;
        }
    }
}
