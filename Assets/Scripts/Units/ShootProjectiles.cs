using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectiles : MonoBehaviour
{
    public float offset = 1f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            BulletManager.Instance.GetBullet(this.gameObject.tag);
            // collect event
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Bullet bullet = BulletManager.Instance.GetBullet(this.gameObject.tag);
            Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            // Apply the offset along the direction
            Vector2 offsetPosition = (Vector2)this.transform.position + direction.normalized * offset;

            // Set the bullet's position
            bullet.transform.position = offsetPosition;
            bullet.Shoot(direction);

            Debug.Log("Shooting");
        }
    }
}
