using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform weaponSocket;
    [SerializeField] private Transform hipPose;
    [SerializeField] private Transform aimPose;

    [Header("Aim")]
    [SerializeField] private float aimBlendSpeed = 10f;
    [SerializeField] private string aimLayerName = "UpperBody_Aim";

    [Header("Aim Raycast (Camera -> AimPoint)")]
    [SerializeField] private Camera aimCamera;
    [SerializeField] private float maxAimDistance = 250f;
    [SerializeField] private LayerMask aimMask = ~0;

    private int aimLayerIndex = -1;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        if (animator != null)
            aimLayerIndex = animator.GetLayerIndex(aimLayerName);

        if (aimCamera == null)
            aimCamera = Camera.main;
    }

    void Update()
    {
        bool isAiming = Input.GetMouseButton(1);

        float current = (aimLayerIndex >= 0) ? animator.GetLayerWeight(aimLayerIndex) : 0f;
        float target = isAiming ? 1f : 0f;
        float next = Mathf.MoveTowards(current, target, aimBlendSpeed * Time.deltaTime);

        if (aimLayerIndex >= 0)
            animator.SetLayerWeight(aimLayerIndex, next);

        ApplyWeaponPose(next);
    }

    private void ApplyWeaponPose(float aimWeight)
    {
        if (weaponSocket == null || hipPose == null || aimPose == null) return;

        weaponSocket.localPosition = Vector3.Lerp(hipPose.localPosition, aimPose.localPosition, aimWeight);
        weaponSocket.localRotation = Quaternion.Slerp(hipPose.localRotation, aimPose.localRotation, aimWeight);
    }

    public Vector3 GetAimPoint()
    {
        if (aimCamera == null) aimCamera = Camera.main;

        Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
            return hit.point;

        return ray.origin + ray.direction * maxAimDistance;
    }
}