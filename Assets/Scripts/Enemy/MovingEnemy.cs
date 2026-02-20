using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MovingEnemy : Enemy
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float moveDistance = 5f;

    [Header("Obstacle Detection")]
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private float wallCheckHeight = 0.6f;
    [SerializeField] private float turnCooldown = 0.25f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform modelToRotate;

    private static readonly int SpeedParam = Animator.StringToHash("Speed");

    private Rigidbody rb;
    private Collider col;

    private Vector3 startPosition;
    private bool movingRight = true;
    private float nextAllowedTurnTime;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        rb.useGravity = true;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;

        startPosition = rb.position;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (modelToRotate == null && animator != null) modelToRotate = animator.transform;
    }

    private void FixedUpdate()
    {
        // 1) Patrol bounds
        float rightLimit = startPosition.x + moveDistance;
        float leftLimit = startPosition.x - moveDistance;

        if (movingRight && rb.position.x >= rightLimit) TryTurn();
        else if (!movingRight && rb.position.x <= leftLimit) TryTurn();

        if (IsBlockedAhead())
            TryTurn();

        float dir = movingRight ? 1f : -1f;
        Vector3 v = rb.velocity;
        v.x = dir * moveSpeed;
        rb.velocity = v;

        // 4) Animation + rotation
        if (animator != null) animator.SetFloat(SpeedParam, Mathf.Abs(rb.velocity.x));

        if (modelToRotate != null)
        {
            float y = movingRight ? 90f : -90f;
            modelToRotate.rotation = Quaternion.Euler(0f, y, 0f);
        }
    }

    private bool IsBlockedAhead()
    {
        Vector3 dir = movingRight ? Vector3.right : Vector3.left;

        Vector3 origin = rb.position + Vector3.up * wallCheckHeight;

        float halfWidth = Mathf.Max(0.15f, col.bounds.extents.x);
        Vector3 halfExtents = new Vector3(halfWidth, 0.3f, Mathf.Max(0.15f, col.bounds.extents.z));

        return Physics.BoxCast(
            origin,
            halfExtents,
            dir,
            out _,
            Quaternion.identity,
            wallCheckDistance,
            obstacleMask,
            QueryTriggerInteraction.Ignore
        );
    }

    private void TryTurn()
    {
        if (Time.time < nextAllowedTurnTime) return;

        movingRight = !movingRight;
        nextAllowedTurnTime = Time.time + turnCooldown;
    }
}