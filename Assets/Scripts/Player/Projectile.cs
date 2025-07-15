using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Projectile : MonoBehaviour
{
    public int damage = 50;
    public LayerMask enemyLayer;
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //FeedbackManager.Instance.PlayHitFeedback(transform.position);
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                IDamageable target = collision.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            //FeedbackManager.Instance.PlayMissFeedback(transform.position);
        }
    }
}
