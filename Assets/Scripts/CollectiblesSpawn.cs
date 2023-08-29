using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollectiblesSpawn : MonoBehaviour
{
    public GameObject[] collectiblePrefabs;
    public Tilemap groundTilemap;
    public float spawnHeightAboveTile = 1.5f;
    public int maxCollectibles = 100;
    public int requiredCollectibles = 70;
    public int tileSpacing = 5; // Number of tiles to skip before attempting another spawn

    private int spawnedCollectibles = 0;

    void Start()
    {
        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        TileBase[] allTiles = groundTilemap.GetTilesBlock(bounds);

        int tileCount = 0; // Count tiles we've gone over

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    tileCount++;

                    if (tileCount % tileSpacing != 0) // Only try to spawn every 'tileSpacing' tiles
                        continue;

                    Vector3Int cellPosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);
                    Vector3 worldPos = groundTilemap.GetCellCenterWorld(cellPosition);

                    Vector2 spawnPosition = new Vector2(worldPos.x, worldPos.y + spawnHeightAboveTile);

                    RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.up, 0.1f);

                    if (hit.collider == null)
                    {
                        int randomCollectibleIndex = Random.Range(0, collectiblePrefabs.Length);
                        Instantiate(collectiblePrefabs[randomCollectibleIndex], spawnPosition, Quaternion.identity);
                        spawnedCollectibles++;

                        if (spawnedCollectibles >= maxCollectibles)
                            return;
                    }
                }
            }
        }
    }
}
