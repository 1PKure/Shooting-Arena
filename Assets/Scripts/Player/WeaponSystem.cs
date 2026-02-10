using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
    }

    

    [SerializeField] private Weapon[] weapons;
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadHintText;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Collider ownerCollider;

    private float nextTimeToFire = 0f;

    void Start()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].model.SetActive(i == currentWeaponIndex);
        }
        NotifyUI();
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

        if (weapons[currentWeaponIndex].projectilePrefab != null)
        {
            ShootProjectile();
        }
        else
        {
            ShootRaycast();
        }
    }

    private void ShootRaycast()
    {
        var w = weapons[currentWeaponIndex];

        if (w.muzzleVFXPrefab != null)
        {
            Transform muzzle = w.firePoint != null ? w.firePoint : playerCamera.transform;
            Instantiate(w.muzzleVFXPrefab, muzzle.position, muzzle.rotation);
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, w.range, ~0, QueryTriggerInteraction.Ignore))
        {
            IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
            if (target != null && ((enemyLayer.value & (1 << hit.collider.gameObject.layer)) != 0))
            {
                target.TakeDamage(w.damage);
                FeedbackManager.Instance.PlayHitFeedback(hit.point, null, w.hitVFXPrefab);
            }
            else
            {
                FeedbackManager.Instance.PlayMissFeedback(hit.point, null, w.missVFXPrefab);
            }
        }
        else
        {
            Vector3 missPoint = ray.origin + ray.direction * w.range;
            FeedbackManager.Instance.PlayMissFeedback(missPoint, null, w.missVFXPrefab);
        }

        FeedbackManager.Instance.PlayShootFeedback(w.shootSFX);
    }


    void ShootProjectile()
    {
        var w = weapons[currentWeaponIndex];

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        Vector3 aimPoint = ray.origin + ray.direction * w.range;
        if (Physics.Raycast(ray, out RaycastHit hit, w.range, ~0, QueryTriggerInteraction.Ignore))
            aimPoint = hit.point;

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
