using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float spawnRate = 10f;

    // Start is called before the first frame update

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(-4f, 4f);
        Vector2 spawnPosition = new Vector2(randomX, transform.position.y);

        GameObject enemy = EnemyPooler.Instance.GetEnemy();

        if (enemy != null)
        {
            enemy.transform.position = spawnPosition;
        }
        else
        {
            Debug.Log("No enemies");
        }
    }
}
