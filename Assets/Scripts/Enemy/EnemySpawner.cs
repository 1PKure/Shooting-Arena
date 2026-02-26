using UnityEngine;
using System.Collections.Generic;
using static GameManager;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public List<Transform> spawnPoints = new List<Transform>();

    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    [Header("Spawn Validation")]
    [SerializeField] private float spawnCheckRadius = 1.0f;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int maxSpawnPointTries = 16;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayUp = 5f;
    [SerializeField] private float rayDown = 20f;
    [SerializeField] private float epsilon = 0.02f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private Difficulty currentDifficulty;
    private bool allowSpawn = true;

    void Update()
    {
        if (!allowSpawn) return;

        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (activeEnemies.Count < maxEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    public void StopSpawning() => allowSpawn = false;

    void SpawnEnemy()
    {
        EnemyData selectedData = GetRandomEnemyData();
        if (selectedData == null || selectedData.prefab == null) return;
        if (spawnPoints == null || spawnPoints.Count == 0) return;

        Transform spawnPoint = GetFreeSpawnPoint();

        if (spawnPoint == null)
        {
            return;
        }

        GameObject enemy = SpawnAligned(selectedData.prefab, spawnPoint.position, spawnPoint.rotation);
        activeEnemies.Add(enemy);
    }

    public void DespawnAllEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
                Destroy(activeEnemies[i]);
        }

        activeEnemies.Clear();

 
        var all = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in all)
            Destroy(e);
    }
    Transform GetFreeSpawnPoint()
    {
        int tries = Mathf.Min(maxSpawnPointTries, spawnPoints.Count);

        for (int i = 0; i < tries; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
            if (sp == null) continue;

            bool occupied = Physics.CheckSphere(sp.position, spawnCheckRadius, enemyLayerMask, QueryTriggerInteraction.Ignore);
            if (!occupied)
                return sp;
        }

        return null;
    }
    public void SetDifficulty(Difficulty difficulty) => currentDifficulty = difficulty;

    public void ResetSpawner()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Destroy(enemy);

        activeEnemies.Clear();
        nextSpawnTime = Time.time + spawnInterval;
        allowSpawn = true;
    }


    EnemyData GetRandomEnemyData()
    {
        List<EnemyData> filtered = enemyDataList.FindAll(data =>
            (currentDifficulty == Difficulty.Easy && data.enemyType == EnemyTypeEnum.Static) ||
            (currentDifficulty == Difficulty.Medium && data.enemyType != EnemyTypeEnum.Shooter) ||
            (currentDifficulty == Difficulty.Hard));

        if (filtered.Count == 0) return null;

        int totalWeight = 0;
        foreach (var data in filtered) totalWeight += data.weight;

        int rand = Random.Range(0, totalWeight);
        int acc = 0;

        foreach (var data in filtered)
        {
            acc += data.weight;
            if (rand < acc)
                return data;
        }

        return null;
    }

    private GameObject SpawnAligned(GameObject prefab, Vector3 approxPos, Quaternion rot)
    {
        Vector3 origin = approxPos + Vector3.up * rayUp;

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayUp + rayDown, groundMask, QueryTriggerInteraction.Ignore))
            return Instantiate(prefab, approxPos, rot);

        GameObject go = Instantiate(prefab, approxPos, rot);

        CapsuleCollider cap = go.GetComponent<CapsuleCollider>();
        if (cap == null) cap = go.GetComponentInChildren<CapsuleCollider>();

        if (cap != null)
        {
            float bottomY = cap.bounds.min.y;
            float rootY = go.transform.position.y;
            float bottomOffset = rootY - bottomY;

            go.transform.position = new Vector3(
                approxPos.x,
                hit.point.y + bottomOffset + epsilon,
                approxPos.z
            );

            Physics.SyncTransforms();
        }
        else
        {
            go.transform.position = new Vector3(approxPos.x, hit.point.y + epsilon, approxPos.z);
            Physics.SyncTransforms();
        }

        return go;
    }

    Bounds GetWorldBounds(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
            return b;
        }

        var colliders = go.GetComponentsInChildren<Collider>();
        if (colliders.Length > 0)
        {
            Bounds b = colliders[0].bounds;
            for (int i = 1; i < colliders.Length; i++) b.Encapsulate(colliders[i].bounds);
            return b;
        }

        return new Bounds(go.transform.position, Vector3.zero);
    }

    public void SetTrainingMode(bool enabled)
    {
        if (!enabled) return;

        maxEnemies = 1;

        spawnInterval = Mathf.Max(0.15f, spawnInterval);

        allowSpawn = true;
        nextSpawnTime = Time.time + 0.05f;
    }

    public void ResumeSpawning()
    {
        allowSpawn = true;
    }
}
