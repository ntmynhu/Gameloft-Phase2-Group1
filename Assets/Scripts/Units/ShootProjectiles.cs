using UnityEngine;
using UnityEngine.Pool;

public class ShootProjectiles : MonoBehaviour
{
    [SerializeField] private float offset = 1f;
    [SerializeField, Range(0f, 45f)] private float accuracyRange;

    private Vector2 direction;
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
            direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            direction = Quaternion.AngleAxis(Random.Range(-accuracyRange, accuracyRange), Vector3.forward) * direction;
            // Apply the offset along the direction
            Vector2 offsetPosition = (Vector2)this.transform.position + direction.normalized * offset;

            // Set the bullet's position
            bullet.transform.position = offsetPosition;
            bullet.Shoot(direction);

            Debug.Log("Shooting");
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 mouseDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-accuracyRange, Vector3.forward) * mouseDirection);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(accuracyRange, Vector3.forward) * mouseDirection);
    }
}
