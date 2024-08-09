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

    private void AddLinePosition(Vector3 start, Vector3 end)
    {
        linePositions.Add(start);
        linePositions.Add(end);
    }
    public override void Cast(GameObject Caster)
    {
        if (state == SkillState.ready)
        {
            countdownTime = castTime;
            Bullet[] bullets = FindObjectsOfType<Bullet>();
            foreach (Bullet bullet in bullets)
            {
                if (bullet != null)
                {
                    if (bullet.IsCollectable)
                    {
                        // Visualize the line in the Scene view
                        AddLinePosition(bullet.transform.position, Caster.transform.position);
                    }
                }
            }
            isCasted = false;
        }
        if (countdownTime <= 0)
        {
            isCasted = true;
            this.SetActive();
            countdownTime = castTime;
        }
        else
        {
            countdownTime -= Time.deltaTime;
        }
    }

    public override void UpdateAimSprite(AimRenderer aimRenderer)
    {
        aimRenderer.AimLineRenderer.enabled = true;
        aimRenderer.AimLineRenderer.positionCount = linePositions.Count;

        for (int i = 0; i < linePositions.Count; i++)
        {
            aimRenderer.AimLineRenderer.SetPosition(i, linePositions[i]);
        }
    }
    public override void Activate(GameObject Caster)
    {

        Debug.Log("Summonskill Activate");

        Player_SkillTest main = Caster.GetComponent<Player_SkillTest>();

        //Find each bullet and try a Linecast
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        Debug.Log(bullets.Length);
        foreach (Bullet bullet in bullets)
        {
            if (bullet != null)
            {
                if (bullet.IsCollectable)
                {
                    // Perform the Linecast
                    RaycastHit2D[] hit = Physics2D.LinecastAll(bullet.transform.position, Caster.transform.position);

                    // Check if the line hit something
                    if (hit.Length > 0)
                    {
                        Debug.Log("Hit objects: " + hit.Length);
                        for (int i = 0; i < hit.Length; i++)
                        {
                            Debug.Log("Hit: " + hit[i].collider.name);
                            //Change enemy's health
                            if (hit[i].collider.TryGetComponent<Enemy_BulletTest>(out Enemy_BulletTest enemy))
                            {
                                enemy.TakeDmg(dmg);
                            }
                        }
                    }
                    Collider2D bulletCollide = bullet.GetComponent<Collider2D>();
                    bulletCollide.enabled = false; //Turn off the collision of the bullet, as we use raycast instead 

                    //Move the bullet back to Caster (doTween)
                    bullet.transform.DOMove(Caster.transform.position, activeDuration)
                        .SetEase(Ease.Linear) // Set movement to linear (no acceleration/deceleration)
                        .OnComplete(() => OnMovementComplete(bullet));

                    //main.MoveBulletToPlayer(bullet, main.gameObject.transform.position, activeDuration);
                    main.Heal(1);
                }

            }
        }
    }

    private void OnMovementComplete(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        linePositions.Clear();
        SetDisabled();
    }
}
