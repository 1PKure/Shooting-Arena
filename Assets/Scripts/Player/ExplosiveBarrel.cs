using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 40;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 80;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private LayerMask damageableMask;
    [SerializeField] private GameObject explosionVFXPrefab;

    [Header("Reward")]
    [SerializeField] private int rewardScore = 0;

    private int currentHealth;
    private bool exploded;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (exploded || amount <= 0)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
            Explode();
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        if (explosionVFXPrefab != null)
            Instantiate(explosionVFXPrefab, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageableMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.root == transform.root)
                continue;

            IDamageable damageable = hits[i].GetComponentInParent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(explosionDamage);

            Rigidbody rb = hits[i].attachedRigidbody;
            if (rb != null && !rb.isKinematic)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        if (rewardScore > 0 && GameManager.Instance != null)
            GameManager.Instance.AddScore(rewardScore);

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}