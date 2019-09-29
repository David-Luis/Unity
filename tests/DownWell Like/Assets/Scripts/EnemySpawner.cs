using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] m_landEnemyPrefabs;
    public GameObject[] m_airEnemyPrefabs;

    public void SpawnEnemies(ChunkController chunkController)
    {
        SpawnEnemy(GetRandomPosition(chunkController.landSpawners), GetRandomLandEnemy());
        SpawnEnemy(GetRandomPosition(chunkController.airSpawners), GetRandomAirEnemy());
    }

    private Vector3 GetRandomPosition(List<BoxCollider2D> spawners)
    {
        BoxCollider2D randomSpawner = spawners[Random.Range(0, spawners.Count - 1)];
        return new Vector3(randomSpawner.transform.position.x, randomSpawner.transform.position.y - randomSpawner.size.y * 0.5f, randomSpawner.transform.position.z);
    }

    private GameObject GetRandomLandEnemy()
    {
        return m_landEnemyPrefabs[0];
    }

    private GameObject GetRandomAirEnemy()
    {
        return m_airEnemyPrefabs[0];
    }

    private void SpawnEnemy(Vector3 position, GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity, this.transform);
    }
}
