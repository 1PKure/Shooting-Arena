using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 100;

    [Tooltip("Qué capas pueden recibir daño (enemigos, destructibles, etc.)")]
    [SerializeField] private LayerMask damageableMask;

    [Tooltip("Qué capas disparan la explosión (terreno, paredes, enemigos, etc.)")]
    [SerializeField] private LayerMask collisionMask;

    [SerializeField] private GameObject explosionEffect;

    private bool hasExploded;

    public void SetDamage(int dmg) => explosionDamage = dmg;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;

        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, damageableMask);

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


