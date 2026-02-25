using UnityEngine;

public class ShooterEnemy : Enemy
{
    public float detectionRange = 15f;
    public float fireRate = 1f;
    public int damage = 10;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform modelToRotate;
    private static readonly int ShootParam = Animator.StringToHash("Shoot");
    private static readonly int IsAimingParam = Animator.StringToHash("IsAiming");

    private Transform player;
    private float nextTimeToFire = 0f;

    protected override void Start()
    {
        base.Start();

        var playerGo = GameObject.FindGameObjectWithTag("Player");
        player = playerGo != null ? playerGo.transform : null;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
        if (modelToRotate == null && animator != null)
            modelToRotate = animator.transform;
    }

    void Update()
    {
        if (player == null) return;

        bool inRange = Vector3.Distance(transform.position, player.position) <= detectionRange;
        if (animator != null) animator.SetBool(IsAimingParam, inRange);

        if (!inRange) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        Quaternion look = Quaternion.LookRotation(direction);
        if (modelToRotate != null) modelToRotate.rotation = look;
        else transform.rotation = look;

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (animator != null) animator.SetTrigger(ShootParam);

        Vector3 direction = (player.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        projectile.layer = LayerMask.NameToLayer("Projectile");

        Collider myCol = GetComponent<Collider>();
        Collider projCol = projectile.GetComponent<Collider>();
        if (myCol != null && projCol != null)
            Physics.IgnoreCollision(projCol, myCol);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = direction.normalized * projectileSpeed;
        }

        Destroy(projectile, 3f);
    }
}