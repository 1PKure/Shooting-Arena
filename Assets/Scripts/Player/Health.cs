using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool isPlayer = false;
    [SerializeField] private int scoreOnDeath = 1;

    public event Action<int, int> OnHealthChanged; 
    public event Action<int> OnDamaged;            
    public event Action<int> OnHealed;             
    public event Action OnDied;

    private int currentHealth;
    public bool IsAlive => currentHealth > 0;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        if (isPlayer && GameManager.Instance.IsGodMode())
            return;

        int prev = currentHealth;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != prev)
        {
            OnDamaged?.Invoke(amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        int prev = currentHealth;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth != prev)
        {
            OnHealed?.Invoke(amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        if (isPlayer && UIManager.Instance != null)
            UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void Die()
    {
        OnDied?.Invoke();

        if (isPlayer)
        {
            Debug.Log("Jugador muerto");
            GameManager.Instance.GameOver();
            return;
        }

        FeedbackManager.Instance.PlayDeathFeedback(transform.position);
        GameManager.Instance.RegisterKill(scoreOnDeath);

        Destroy(transform.root.gameObject);
    }
}
