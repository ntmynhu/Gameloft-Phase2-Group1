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

    public void GetBulletToTransform(Vector3 center, float radius, string tag, int number)
    {
        StartCoroutine(SpawnAndAnimateBullets(center, radius, tag, number));
    }

    private IEnumerator MoveBulletTowardsCenter(Bullet bullet, Vector3 center, float duration)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 startPosition = bullet.transform.position;
        float elapsedTime = 0f;

        // Move the bullet towards the center
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            bullet.transform.position = Vector3.Lerp(startPosition, center, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the bullet ends exactly at the center
        bullet.transform.position = center;

        // Now start spreading the bullet
        StartCoroutine(SpreadBullet(bullet, center));
    }
    private IEnumerator SpreadBullet(Bullet bullet, Vector3 center)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Calculate a random direction for spreading out
            Vector3 direction = Random.insideUnitCircle.normalized;

            // Apply a spread effect
            rb.velocity = direction * Random.Range(3, 10);

            // Wait for a short duration before stopping the bullet
            yield return new WaitForSeconds(0.5f);

            // Stop the bullet by setting its velocity to zero
            rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator SpawnAndAnimateBullets(Vector3 center, float radius, string tag, int number)
    {
        
        for (int i = 0; i < number; i++)
        {
            Bullet bullet = GetBullet(tag);
            Vector3 initialPosition = center;
            initialPosition.y = initialPosition.y + 3;

            bullet.transform.position = initialPosition;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Move bullets towards the center
                StartCoroutine(MoveBulletTowardsCenter(bullet, center, 0.1f));
            }
        }
        yield return null;
    }

}
