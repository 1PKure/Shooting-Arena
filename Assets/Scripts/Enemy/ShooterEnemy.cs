using UnityEngine;

public class ShooterEnemy : Enemy
{
    public float detectionRange = 15f;
    public float fireRate = 1f;
    public int damage = 10;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    private Transform player;
    private float nextTimeToFire = 0f;

    protected override void Start()
    {
        base.Start();
        maxHealth = 100;
        currentHealth = maxHealth;
        pointValue = 30;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!isActive) return;

        
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            
            Vector3 direction = player.position - transform.position;
            direction.y = 0; 
            transform.rotation = Quaternion.LookRotation(direction);

            
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        
        Vector3 direction = (player.position - firePoint.position).normalized;
        direction += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

        
        rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);

        
        Destroy(projectile, 5f);
    }
}