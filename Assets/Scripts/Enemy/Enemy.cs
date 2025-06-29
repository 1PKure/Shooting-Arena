using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsData stats;
    protected int currentHealth;
    protected bool isActive = true;

    protected virtual void Start()
    {
        currentHealth = stats.maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        if (!isActive) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isActive = false;
        FeedbackManager.Instance.PlayDeathFeedback(transform.position);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(stats.pointValue);
        }

        Destroy(gameObject);
    }
}
