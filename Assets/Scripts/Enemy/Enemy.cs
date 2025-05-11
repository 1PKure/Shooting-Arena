using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int pointValue = 10;

    public float respawnTime = 5f;
    public bool isActive = true;

    public GameObject hitEffect;
    public GameObject deathEffect;

    protected GameManager gameManager;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        if (currentHealth <= 0 && isActive)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isActive = false;

        
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        
        if (gameManager != null)
        {
            gameManager.AddScore(pointValue);
        }

        
        gameObject.SetActive(false);

        
        Invoke("Respawn", respawnTime);
    }

    protected virtual void Respawn()
    {
        currentHealth = maxHealth;
        isActive = true;
        gameObject.SetActive(true);
    }
}