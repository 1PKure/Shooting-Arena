using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 50;
    public LayerMask enemyLayer;

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }


    }
}
