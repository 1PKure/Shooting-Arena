using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 100;

    [Tooltip("Which layers can receive damage (enemies, destructibles, etc.)")]
    [SerializeField] private LayerMask damageableMask;

    [Tooltip("Which layers trigger the explosion (ground, walls, enemies, etc.)")]
    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private GameObject explosionEffect;

    private bool hasExploded;

    private void OnEnable()
    {
        hasExploded = false;
    }

    public void SetDamage(int dmg) => explosionDamage = dmg;
    public void SetRadius(float radius) => explosionRadius = radius;

    public void SetMasks(LayerMask damageables, LayerMask collisions)
    {
        damageableMask = damageables;
        collisionMask = collisions;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        if (collisionMask.value != 0)
        {
            int other = 1 << collision.gameObject.layer;
            if ((collisionMask.value & other) == 0)
                return;
        }

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;

        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageableMask,
            QueryTriggerInteraction.Ignore
        );

        for (int i = 0; i < hits.Length; i++)
        {
            IDamageable target = hits[i].GetComponentInParent<IDamageable>();
            if (target != null)
                target.TakeDamage(explosionDamage);
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}