using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] m_landEnemyPrefabs;

    public void SpawnEnemies(ChunkController chunkController)
    {
        SpawnEnemy(GetRandomLandPosition(chunkController.landSpawners), GetRandomLandEnemy());
    }

    private Vector3 GetRandomLandPosition(List<BoxCollider2D> landSpawners)
    {
        BoxCollider2D randomLandSpawner = landSpawners[Random.Range(0, landSpawners.Count - 1)];
        return new Vector3(randomLandSpawner.transform.position.x, randomLandSpawner.transform.position.y - randomLandSpawner.size.y * 0.5f, randomLandSpawner.transform.position.z);
    }

    private GameObject GetRandomLandEnemy()
    {
        return m_landEnemyPrefabs[0];
    }

    private void SpawnEnemy(Vector3 position, GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
