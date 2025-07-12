using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        public AudioClip fireSound;
        public ParticleSystem muzzleFlash;
    }

    [SerializeField] private Weapon[] weapons;
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI reloadHintText;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask enemyLayer;

    private float nextTimeToFire = 0f;
    private bool isFiring = false;
    private int damage = 50;

    void Start()
    {
        
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].model.SetActive(i == currentWeaponIndex);
        }
    }

    void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1) EquipWeapon(1);


        if (weapons[currentWeaponIndex].isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && weapons[currentWeaponIndex].currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / weapons[currentWeaponIndex].fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && weapons[currentWeaponIndex].currentAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / weapons[currentWeaponIndex].fireRate;
                Shoot();
            }
        }

        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
    void UpdateUI()
    {
        if (ammoText != null)
            ammoText.text = "Bullets: " + weapons[currentWeaponIndex].currentAmmo;

        if (reloadHintText != null)
            reloadHintText.gameObject.SetActive(weapons[currentWeaponIndex].currentAmmo == 0);
    }
    void EquipWeapon(int index)
    {
        if (index == currentWeaponIndex) return;

        foreach (var weapon in weapons)
        {
            weapon.model.SetActive(false);
        }

        currentWeaponIndex = index;

        weapons[currentWeaponIndex].model.SetActive(true);
    }


    void SwitchWeapon()
    {
        foreach (var weapon in weapons)
        {
            weapon.model.SetActive(false);
        }

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        weapons[currentWeaponIndex].model.SetActive(true);

        Debug.Log("Switched to: " + weapons[currentWeaponIndex].name);
    }


    void Shoot()
    {
        weapons[currentWeaponIndex].currentAmmo--;

        
        if (weapons[currentWeaponIndex].muzzleFlash != null)
        {
            weapons[currentWeaponIndex].muzzleFlash.Play();
        }
        
        
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
            Debug.Log("Hit: " + hit.transform.name);

            
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                FeedbackManager.Instance.PlayHitFeedback(hit.point);
                enemy.TakeDamage(weapons[currentWeaponIndex].damage);
            }
            else
            {
                FeedbackManager.Instance.PlayMissFeedback(hit.point);
            }
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(
            weapons[currentWeaponIndex].projectilePrefab,
            weapons[currentWeaponIndex].firePoint.position,
            weapons[currentWeaponIndex].firePoint.rotation
        );

        projectile.transform.forward = playerCamera.transform.forward;

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.forward * weapons[currentWeaponIndex].projectileSpeed, ForceMode.Impulse);
        FeedbackManager.Instance.PlayShootFeedback();
        Destroy(projectile, 3f);
    }

    void Reload()
    {
        weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].maxAmmo;
    }

    public void ResetWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].currentAmmo = weapons[i].maxAmmo;
            weapons[i].model.SetActive(i == 0);
        }
        currentWeaponIndex = 0;
    }


}