using UnityEngine;

public class ProjectileDetector : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile != null)
            {
                player.TakeDamage(projectile.damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
