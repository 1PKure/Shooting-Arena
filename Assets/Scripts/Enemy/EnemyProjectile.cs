using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    [Header("Destroy Layer")]
    [SerializeField] private LayerMask destroyOnHitMask;

    private void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGodMode())
            {
                Destroy(gameObject);
                return;
            }

            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                UIManager.Instance?.PlayDamageOverlay();
            }

            Destroy(gameObject);
            return;
        }

        if (((1 << other.gameObject.layer) & destroyOnHitMask) != 0)
        {
            Destroy(gameObject);
        }
    }
}