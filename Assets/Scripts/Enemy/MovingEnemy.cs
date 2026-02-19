using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingEnemy : Enemy
{
    public float moveSpeed = 3f;
    public float moveDistance = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform modelToRotate;

    private static readonly int SpeedParam = Animator.StringToHash("Speed");

    private Rigidbody rb;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    private Vector3 lastPosition;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        startPosition = rb.position;
        lastPosition = rb.position;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (modelToRotate == null && animator != null) modelToRotate = animator.transform;
    }

    private void FixedUpdate()
    {
        targetPosition = movingRight
            ? startPosition + Vector3.right * moveDistance
            : startPosition - Vector3.right * moveDistance;

        Vector3 next = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        float moved = Vector3.Distance(rb.position, lastPosition);
        float speed = (Time.fixedDeltaTime > 0f) ? (moved / Time.fixedDeltaTime) : 0f;
        if (animator != null) animator.SetFloat(SpeedParam, speed);

        Vector3 delta = rb.position - lastPosition;
        if (modelToRotate != null && delta.sqrMagnitude > 0.0001f)
        {
            float y = (delta.x >= 0f) ? 90f : -90f;
            modelToRotate.rotation = Quaternion.Euler(0f, y, 0f);
        }

        lastPosition = rb.position;

        if (Vector3.Distance(rb.position, targetPosition) < 0.05f)
            movingRight = !movingRight;
    }
}