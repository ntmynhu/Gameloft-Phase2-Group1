using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    [SerializeField] Bullet bullletPref;
    private static IObjectPool<Bullet> bulletPool;
    [SerializeField] private bool collectionCheck;

    //Singleton
    public static BulletManager Instance { get; set; } 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        bulletPool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet, DestroyBullet, collectionCheck);
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bullletPref);
        return bullet;
    }

    private void ReleaseBullet(Bullet bullet)
    {
        bullet.enabled = false;
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.enabled = true;
    }
    private void DestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public Bullet GetBullet(string tag)
    {
        Bullet bullet = bulletPool.Get();
        bullet.tag = tag;
        return bullet;
    }    
}
