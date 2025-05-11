using UnityEngine;
using System.Collections.Generic;
using static GameManager;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public int weight = 1;
    }

    private Difficulty currentDifficulty;
    public List<EnemyType> enemyTypes = new List<EnemyType>();
    public List<Transform> spawnPoints = new List<Transform>();

    public int maxEnemies = 10;
    public float spawnInterval = 2f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime = 0f;

    void Update()
    {

        if (activeEnemies.Count < maxEnemies && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }


        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
    }

    void SpawnEnemy()
    {

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];


        GameObject enemyPrefab = GetRandomEnemyPrefab();

        if (enemyPrefab != null && spawnPoint != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            activeEnemies.Add(enemy);
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        this.currentDifficulty = difficulty;
    }


    GameObject GetRandomEnemyPrefab()
    {
        var filtered = enemyTypes.FindAll(et =>
        (currentDifficulty == Difficulty.Easy && et.enemyPrefab.name.Contains("Static")) ||
        (currentDifficulty == Difficulty.Medium && et.enemyPrefab.name.Contains("Mover")) ||
        (currentDifficulty == Difficulty.Hard));

        if (filtered.Count == 0) return null;

        int totalWeight = 0;

        foreach (var et in filtered) totalWeight += et.weight;

        int rand = Random.Range(0, totalWeight);
        int acc = 0;
        foreach (var et in filtered)
        {
            acc += et.weight;
            if (rand < acc)
                return et.enemyPrefab;
        }

        return null;
    }
}