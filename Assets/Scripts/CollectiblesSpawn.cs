using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectiblesSpawn : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    public Tilemap groundTilemap;
    public float spawnHeightAboveTile = 1.5f;
    public int minCollectibles = 100; // Changed from maxCollectibles to minCollectibles
    public int tileSpacing = 5;

    private int spawnedCollectibles = 0;

    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        TileBase[] allTiles = groundTilemap.GetTilesBlock(bounds);

        List<Vector2> possibleSpawnPoints = new List<Vector2>();

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    Vector3Int cellPosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                    Vector3 worldPos = groundTilemap.GetCellCenterWorld(cellPosition);

                    Vector2 spawnPosition = new Vector2(worldPos.x, worldPos.y + spawnHeightAboveTile);

                    RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.up, 0.1f);

                    if (hit.collider == null)
                    {
                        possibleSpawnPoints.Add(spawnPosition);
                    }
                }
            }
        }

        Debug.Log("Possible spawn points: " + possibleSpawnPoints.Count);

        while (spawnedCollectibles < minCollectibles && possibleSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleSpawnPoints.Count);
            Vector2 randomSpawnPoint = possibleSpawnPoints[randomIndex];
            possibleSpawnPoints.RemoveAt(randomIndex);

            int randomCollectibleIndex = Random.Range(0, collectiblePrefabs.Length);
            Instantiate(collectiblePrefabs[randomCollectibleIndex], randomSpawnPoint, Quaternion.identity);
            spawnedCollectibles++;
        }
    }

}
