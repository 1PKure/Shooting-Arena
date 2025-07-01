using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 50;
    public LayerMask enemyLayer;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //FeedbackManager.Instance.PlayHitFeedback(transform.position);
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            //FeedbackManager.Instance.PlayMissFeedback(transform.position);
        }
    }
}
