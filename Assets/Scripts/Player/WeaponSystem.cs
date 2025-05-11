using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public Weapon[] weapons;
    public int currentWeaponIndex = 0;

    public Camera playerCamera;
    public LayerMask enemyLayer;

    private float nextTimeToFire = 0f;
    private bool isFiring = false;

    void Start()
    {
        
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].model.SetActive(i == currentWeaponIndex);
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }

        
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

    void SwitchWeapon()
    {
        
        weapons[currentWeaponIndex].model.SetActive(false);

        
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        
        weapons[currentWeaponIndex].model.SetActive(true);
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
                enemy.TakeDamage(weapons[currentWeaponIndex].damage);
            }
        }
    }

    void ShootProjectile()
    {
        
        GameObject projectile = Instantiate(weapons[currentWeaponIndex].projectilePrefab, weapons[currentWeaponIndex].firePoint.position, weapons[currentWeaponIndex].firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        
        rb.AddForce(playerCamera.transform.forward * weapons[currentWeaponIndex].projectileSpeed, ForceMode.Impulse);

        
        Destroy(projectile, 3f);
    }

    void Reload()
    {
        weapons[currentWeaponIndex].currentAmmo = weapons[currentWeaponIndex].maxAmmo;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        var list = new List<Weapon>(weapons);
        list.Add(newWeapon);
        weapons = list.ToArray();

        newWeapon.model.SetActive(false);
    }

}