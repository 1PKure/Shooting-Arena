using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBar;
    public TMP_Text healthText;

    public float invincibilityTime = 0.5f;
    private bool isInvincible = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateUI();
        isInvincible = true;
        Invoke("ResetInvincibility", invincibilityTime);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ResetInvincibility()
    {
        isInvincible = false;
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    void Die()
    {
        Debug.Log("Jugador muerto");

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateUI();
    }
}