using Unity.VisualScripting;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    [Header("Rango permitido")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    public float minY = -10f;
    public float maxY = 10f;
    [Header("Comportamiento")]
    [SerializeField] private bool useHardBoundary = true;
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private bool showWarningUI = true;

    [Header("Advertencia Visual")]
    [SerializeField] private GameObject warningUI;
    [SerializeField] private float warningDistance = 5f;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (warningUI != null)
            warningUI.SetActive(false);
    }
    void Update()
    {

        Vector3 currentPos = transform.position;
        Vector3 newPos = currentPos;
        bool nearBoundary = false;


        if (currentPos.x < minX + warningDistance || currentPos.x > maxX - warningDistance ||
            currentPos.z < minZ + warningDistance || currentPos.z > maxZ - warningDistance ||
            currentPos.y < minY + warningDistance || currentPos.y > maxY - warningDistance)
        {
            nearBoundary = true;
        }


        if (showWarningUI && warningUI != null)
        {
            warningUI.SetActive(nearBoundary);
        }
    }

    public Vector3 LimitMovement(Vector3 desiredMove)
    {
        Vector3 currentPos = transform.position;
        Vector3 nextPos = currentPos + desiredMove;

        nextPos.x = Mathf.Clamp(nextPos.x, minX, maxX);
        nextPos.y = Mathf.Clamp(nextPos.y, minY, maxY);
        nextPos.z = Mathf.Clamp(nextPos.z, minZ, maxZ);

        return nextPos - currentPos;
    }
    public bool IsInsideBounds(Vector3 targetPosition)
    {
        return targetPosition.x >= minX && targetPosition.x <= maxX &&
               targetPosition.z >= minZ && targetPosition.z <= maxZ &&
               targetPosition.y >= minY && targetPosition.y <= maxY;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minX + maxX) / 2f, transform.position.y, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), 0.1f, Mathf.Abs(maxZ - minZ));
        Gizmos.DrawWireCube(center, size);
    }
}
