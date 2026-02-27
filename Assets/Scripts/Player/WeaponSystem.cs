using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    [System.Serializable]
    public class Weapon
    {
        public string name;
        public GameObject model;
        public int damage;
        public float fireRate;
        public int maxAmmo;
        public int currentAmmo;
        public bool isAutomatic;
        public float range;
        public Transform firePoint;
        public GameObject projectilePrefab;
        public float projectileSpeed; 
        [Header("VFX/SFX")]
        public GameObject muzzleVFXPrefab;
        public GameObject hitVFXPrefab;
        public GameObject missVFXPrefab;
        public AudioClip shootSFX;
        public float cameraShakeStrength;
        public bool useRaycast;
        [Header("Area Damage (Raycast)")]
        public bool useExplosionAOE;
        public float explosionRadius = 5f;
        public int explosionDamage = 120;
        public bool explosionFalloff = true;
        public GameObject explosionVFXPrefab;
    }


    [SerializeField] private Transform aimSource;
    [SerializeField] private Weapon[] weapons;
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Collider ownerCollider;
    [SerializeField] private PlayerInputReader inputReader;

    private float nextTimeToFire = 0f;





    void Start()
    {
        if (aimSource == null)
            aimSource = playerCamera.transform;

        
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].maxAmmo < 0) weapons[i].maxAmmo = 0;

            
            if (weapons[i].currentAmmo <= 0)
                weapons[i].currentAmmo = weapons[i].maxAmmo;

            if (weapons[i].fireRate <= 0f)
                weapons[i].fireRate = 1f; 
        }

        for (int i = 0; i < weapons.Length; i++)
            weapons[i].model.SetActive(i == currentWeaponIndex);

        NotifyUI();
    }
    private Ray GetAimRay()
    {
        if (playerCamera == null) playerCamera = Camera.main;
        return playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    }
    public void TryShoot()
    {
        if (Time.time >= nextTimeToFire && weapons[currentWeaponIndex].currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / weapons[currentWeaponIndex].fireRate;
            Shoot();
            NotifyUI();
        }
    }
    public void SwitchWeapon()
    {
        foreach (var weapon in weapons)
            weapon.model.SetActive(false);

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        weapons[currentWeaponIndex].model.SetActive(true);

        NotifyUI();
    }

    public void NotifyUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmo(
                weapons[currentWeaponIndex].currentAmmo,
                weapons[currentWeaponIndex].maxAmmo
            );
        }
    }
    public void Reload()
    {
        weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].maxAmmo;
        NotifyUI();
    }

    public void EquipWeapon(int index)
    {
        if (index == currentWeaponIndex || index >= weapons.Length) return;

        foreach (var weapon in weapons)
            weapon.model.SetActive(false);

        currentWeaponIndex = index;
        weapons[currentWeaponIndex].model.SetActive(true);

        NotifyUI();
    }

    public bool IsCurrentWeaponAutomatic()
    {
        return weapons[currentWeaponIndex].isAutomatic;
    }

    private void Shoot()
    {
        weapons[currentWeaponIndex].currentAmmo--;

        var w = weapons[currentWeaponIndex];
        if (w.useRaycast) ShootRaycast();
        else ShootProjectile();
    }

    private void ShootRaycast()
    {
        var w = weapons[currentWeaponIndex];

        Ray ray = GetAimRay();

        if (w.muzzleVFXPrefab != null)
        {
            Transform muzzle = w.firePoint != null ? w.firePoint : playerCamera.transform;
            Instantiate(w.muzzleVFXPrefab, muzzle.position, muzzle.rotation);
        }


        if (Physics.Raycast(ray, out RaycastHit hit, w.range, ~0, QueryTriggerInteraction.Ignore))
        {
            if (!w.useExplosionAOE)
            {
                IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
                if (ownerCollider != null && hit.collider == ownerCollider)
                    return;
                int hitLayer = hit.collider.gameObject.layer;
                int rootLayer = hit.collider.transform.root.gameObject.layer;

                bool isEnemy =
                    ((1 << hitLayer) & enemyLayer.value) != 0 ||
                    ((1 << rootLayer) & enemyLayer.value) != 0;

                if (target != null && isEnemy)
                    FeedbackManager.Instance.PlayHitFeedback(hit.point, null, w.hitVFXPrefab);
                else
                    FeedbackManager.Instance.PlayMissFeedback(hit.point, null, w.missVFXPrefab);

                if (target != null && isEnemy)
                    target.TakeDamage(w.damage);

                FeedbackManager.Instance.PlayShootFeedback(w.shootSFX);
                return;
            }

            if (w.explosionVFXPrefab != null)
                Instantiate(w.explosionVFXPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            FeedbackManager.Instance.PlayHitFeedback(hit.point, null, w.hitVFXPrefab);

            ApplyExplosionDamage(hit.point, w.explosionRadius, w.explosionDamage, enemyLayer, w.explosionFalloff);

            FeedbackManager.Instance.PlayShootFeedback(w.shootSFX);
        }
        else
        {
            Vector3 missPoint = ray.origin + ray.direction * w.range;
            FeedbackManager.Instance.PlayMissFeedback(missPoint, null, w.missVFXPrefab);
            FeedbackManager.Instance.PlayShootFeedback(w.shootSFX);
        }
    }

    private void ApplyExplosionDamage(Vector3 center, float radius, int baseDamage, LayerMask damageableMask, bool falloff)
    {
        Collider[] hits = Physics.OverlapSphere(center, radius, damageableMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits.Length; i++)
        {
            IDamageable target = hits[i].GetComponentInParent<IDamageable>();
            if (target == null) continue;

            int dmg = baseDamage;

            if (falloff)
            {
                float dist = Vector3.Distance(center, hits[i].ClosestPoint(center));
                float t = Mathf.Clamp01(dist / radius);
                dmg = Mathf.RoundToInt(Mathf.Lerp(baseDamage, 0f, t));
            }

            if (dmg > 0)
                target.TakeDamage(dmg);
        }
    }


    void ShootProjectile()
    {
        var w = weapons[currentWeaponIndex];

        Ray ray = GetAimRay();

        Vector3 aimPoint = ray.origin + ray.direction * w.range;
        if (Physics.Raycast(ray, out RaycastHit hit, w.range, ~0, QueryTriggerInteraction.Ignore))
            aimPoint = hit.point;
        if (ownerCollider != null && hit.collider == ownerCollider)
            return;

        Vector3 dir = (aimPoint - w.firePoint.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        GameObject projectileGO = Instantiate(w.projectilePrefab, w.firePoint.position, rot);

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetDamage(w.damage);
            projectile.enemyLayer = enemyLayer;
            projectile.SetOwner(gameObject);
            projectile.SetFeedbackOverrides(null, null, w.hitVFXPrefab, w.missVFXPrefab);
        }

        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = dir * w.projectileSpeed;
            rb.angularVelocity = Vector3.zero;
        }

        if (w.muzzleVFXPrefab != null)
            Instantiate(w.muzzleVFXPrefab, w.firePoint.position, w.firePoint.rotation);

        FeedbackManager.Instance.PlayShootFeedback(w.shootSFX);
        Destroy(projectileGO, 3f);
    }
    public void ResetWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].currentAmmo = weapons[i].maxAmmo;
            weapons[i].model.SetActive(i == 0);
        }
        currentWeaponIndex = 0;
        NotifyUI();
    }
}
