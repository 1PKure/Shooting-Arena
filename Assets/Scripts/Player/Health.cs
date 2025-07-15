using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private int scoreOnDeath = 1;

    private int currentHealth;
    public bool IsAlive => currentHealth > 0;

    void Awake()
    {
        currentHealth = maxHealth;

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isPlayer && GameManager.Instance.IsGodMode())
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void Die()
    {
        if (isPlayer)
        {
            Debug.Log("Jugador muerto");
            GameManager.Instance.GameOver();
        }
        else
        {
            FeedbackManager.Instance.PlayDeathFeedback(transform.position);
            GameManager.Instance.AddScore(scoreOnDeath);
            Destroy(gameObject);
        }
    }
}
