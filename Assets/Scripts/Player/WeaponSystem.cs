using UnityEngine;
using TMPro;

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
    }

    [SerializeField] private Weapon[] weapons;
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadHintText;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask enemyLayer;

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

    void ShootRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, weapons[currentWeaponIndex].range, enemyLayer))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                FeedbackManager.Instance.PlayHitFeedback(hit.point);
                IDamageable target = hit.transform.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.TakeDamage(weapons[currentWeaponIndex].damage);
                }
            }
            else
            {
                FeedbackManager.Instance.PlayMissFeedback(hit.point);
            }
        }
    }

    void ShootProjectile()
    {
        GameObject projectileGO = Instantiate(
            weapons[currentWeaponIndex].projectilePrefab,
            weapons[currentWeaponIndex].firePoint.position,
            weapons[currentWeaponIndex].firePoint.rotation
        );

        projectileGO.transform.forward = playerCamera.transform.forward;
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetDamage(weapons[currentWeaponIndex].damage);
        }

        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
        rb.AddForce(projectileGO.transform.forward * weapons[currentWeaponIndex].projectileSpeed, ForceMode.Impulse);

        FeedbackManager.Instance.PlayShootFeedback();
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
