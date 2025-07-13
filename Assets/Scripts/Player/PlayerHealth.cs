using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image healthBar;
    public TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        if (GameManager.Instance.IsGodMode()) return;
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
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