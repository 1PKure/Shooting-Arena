using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 10;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}