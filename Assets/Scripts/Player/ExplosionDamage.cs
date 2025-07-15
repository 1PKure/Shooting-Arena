using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffect;
    public void SetDamage(int dmg)
    {
        explosionDamage = dmg;
    }

    void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (var hit in hitColliders)
        {
            if (explosionEffect)
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Enemy enemy = hit.GetComponent<Enemy>();
            IDamageable target = hit.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }
}

