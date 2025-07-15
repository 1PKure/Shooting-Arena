using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStatsData stats;

    protected virtual void Start()
    {
        var health = GetComponent<Health>();
        if (health != null)
        {
            health.Heal(stats.maxHealth);
        }
    }

    void IDamageable.TakeDamage(int amount)
    {
        var health = GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(amount);
            if (!health.IsAlive)
            {
                health.Die();
            }
        }
    }

}
