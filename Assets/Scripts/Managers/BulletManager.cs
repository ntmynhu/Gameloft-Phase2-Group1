using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    [SerializeField] Bullet bullletPref;
    public IObjectPool<Bullet> bulletPool;
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
        Debug.Log("bullet released");
        bullet.gameObject.SetActive(false);
    }

    private void GetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
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
