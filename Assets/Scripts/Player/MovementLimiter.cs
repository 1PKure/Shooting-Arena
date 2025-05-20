using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    [Header("Rango permitido")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    public bool IsInsideBounds(Vector3 targetPosition)
    {
        return targetPosition.x >= minX && targetPosition.x <= maxX &&
               targetPosition.z >= minZ && targetPosition.z <= maxZ;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((minX + maxX) / 2f, transform.position.y, (minZ + maxZ) / 2f);
        Vector3 size = new Vector3(Mathf.Abs(maxX - minX), 0.1f, Mathf.Abs(maxZ - minZ));
        Gizmos.DrawWireCube(center, size);
    }
}
