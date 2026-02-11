using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 50;
    public LayerMask enemyLayer;

    [SerializeField] private float speed = 60f;

    private Rigidbody _rb;
    private Vector3 _dir = Vector3.forward;

    private AudioClip hitSfxOverride;
    private AudioClip missSfxOverride;
    private GameObject hitVfxOverride;
    private GameObject missVfxOverride;

    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }
    public void SetDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.0001f) return;

        _dir = direction.normalized;

        transform.rotation = Quaternion.LookRotation(_dir, Vector3.up);

        if (_rb != null)
            _rb.velocity = _dir * speed;
    }
    public void SetDamage(int dmg) => damage = dmg;

    public void SetOwner(GameObject ownerGO)
    {
        if (ownerGO == null || myCollider == null) return;
        foreach (var c in ownerGO.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(myCollider, c, true);
    }

    public void SetFeedbackOverrides(AudioClip hitSfx, AudioClip missSfx, GameObject hitVfx, GameObject missVfx)
    {
        hitSfxOverride = hitSfx;
        missSfxOverride = missSfx;
        hitVfxOverride = hitVfx;
        missVfxOverride = missVfx;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.GetContact(0).point;

        bool isEnemy = ((1 << collision.gameObject.layer) & enemyLayer.value) != 0;

        if (isEnemy)
        {
            IDamageable target = collision.collider.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
                FeedbackManager.Instance.PlayHitFeedback(hitPoint, hitSfxOverride, hitVfxOverride);
            }
            else
            {
                FeedbackManager.Instance.PlayMissFeedback(hitPoint, missSfxOverride, missVfxOverride);
            }
        }
        else
        {
            FeedbackManager.Instance.PlayMissFeedback(hitPoint, missSfxOverride, missVfxOverride);
        }

        Destroy(gameObject);
    }
}