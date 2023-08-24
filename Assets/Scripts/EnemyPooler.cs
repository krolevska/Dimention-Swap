using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public static EnemyPooler Instance; // Singleton pattern for easy access

    public GameObject enemyPrefab;
    public int poolSize = 10; // Number of enemies to instantiate at start
    public bool canGrow = true; // If true, the pool can grow if more enemies are needed than initially instantiated

    private List<GameObject> pooledEnemies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pooledEnemies = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pooledEnemies.Add(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                pooledEnemies[i].SetActive(true);
                return pooledEnemies[i];
            }
        }

        if (canGrow)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(true);  // Make sure the new enemy is active
            pooledEnemies.Add(enemy);
            return enemy;
        }

        return null;
    }

}
