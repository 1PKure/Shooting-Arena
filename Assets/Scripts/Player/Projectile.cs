using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 50;
    public LayerMask enemyLayer;

    [Header("Feedback Overrides (optional)")]
    [SerializeField] private AudioClip hitSfxOverride;
    [SerializeField] private AudioClip missSfxOverride;
    [SerializeField] private GameObject hitVfxOverride;
    [SerializeField] private GameObject missVfxOverride;

    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    public void SetDamage(int dmg) => damage = dmg;

    public void SetOwner(GameObject ownerGO)
    {
        if (ownerGO == null || myCollider == null) return;

        // Ignora toda colisión con el dueño (player), evita disparos raros al salir
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