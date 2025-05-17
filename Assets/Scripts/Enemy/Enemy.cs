using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas")]
    public int maxHealth = 100;
    protected int currentHealth;
    public int pointValue = 10;

    [Header("Estado")]
    protected bool isActive = true;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(pointValue);
        }

        Destroy(gameObject);
    }
}
