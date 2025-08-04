using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Enemy Spawner Settings")]
    [SerializeField] private GameObject containerEnemySpawner;
    [SerializeField] private GameObject[] enemys;
    [SerializeField] private Transform[] spawnerEnemies;
    [SerializeField] private int enemyCaps = 50;

    [Header("Items Spawner Settings")]
    [SerializeField] private GameObject containerItems;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform[] itemSpawner;
    [SerializeField] private int itemCaps = 20;

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 2f;
    private bool stopSpawning = false;

    [SerializeField] private int currEnemyCount = 0;
    [SerializeField] private int currItemCount = 0;

    private void Start()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GetStartSpawning(int required)
    {
        if (spawnerEnemies.Length == 0 || itemSpawner.Length == 0)
        {
            Debug.Log("No spawn points assigned");
            return;
        }

        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("ItemSpawner");
        itemSpawner = new Transform[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; i++)
        {
            itemSpawner[i] = spawnerObjects[i].transform;
        }

        StartCoroutine(SpawningEnemy());
        SpawnItem();
    }

    IEnumerator SpawningEnemy()
    {
        while (!stopSpawning && currEnemyCount < enemyCaps)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
        if (items.Length == 0)
        {
            Debug.LogWarning("There are no Prefabs item");
            return;
        }

        if (currItemCount >= itemCaps)
        {
            Debug.Log("Item cap reached");
            return;
        }

        Transform randomSpawner = itemSpawner[Random.Range(0, itemSpawner.Length)];
        GameObject  randomItem = items[Random.Range(0, items.Length)];
        GameObject newItem = Instantiate(randomItem, randomSpawner.position, Quaternion.identity);
        newItem.transform.parent = containerItems.transform;

        currItemCount++;
    }

    void SpawnEnemy()
    {
        if (currEnemyCount >= enemyCaps) return;

        // Choose random enemy prefab
        GameObject enemyPrefab = enemys[Random.Range(0, enemys.Length)];

        // Choose a balanced spawn point using round-robin or random
        int spawnIndex = currEnemyCount % spawnerEnemies.Length;
        Transform spawnPoint = spawnerEnemies[spawnIndex];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Parent the enemy for cleaner hierarchy
        if (containerEnemySpawner != null)
            enemy.transform.parent = containerEnemySpawner.transform;

        currEnemyCount++;
    }


    public void ReduceEnemyCount()
    {
        currEnemyCount--;
        if (currEnemyCount < enemyCaps && !stopSpawning)
        {
            StartCoroutine(SpawningEnemy()); // Resume spawning if needed
        }
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}
