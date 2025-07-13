using UnityEngine;
using System.Collections.Generic;
using static GameManager;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();
    public List<Transform> spawnPoints = new List<Transform>();

    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private Difficulty currentDifficulty;
    private bool allowSpawn = true;

    void Update()
    {
        if (!allowSpawn) return;
        if (activeEnemies.Count < maxEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }

        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
    }

    public void StopSpawning()
    {
        allowSpawn = false;
    }
    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        EnemyData selectedData = GetRandomEnemyData();

        if (selectedData != null && selectedData.prefab != null)
        {
            GameObject enemy = Instantiate(selectedData.prefab, spawnPoint.position, spawnPoint.rotation);
            activeEnemies.Add(enemy);
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        this.currentDifficulty = difficulty;
    }

    public void ResetSpawner()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
        nextSpawnTime = Time.time + spawnInterval;
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
}
