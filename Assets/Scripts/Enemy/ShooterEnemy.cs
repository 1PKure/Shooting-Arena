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

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 direction = (player.position - firePoint.position).normalized;
        direction += new Vector3(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
        );

        rb.AddForce(direction * projectileSpeed, ForceMode.Impulse);
        Destroy(projectile, 5f);
    }
}