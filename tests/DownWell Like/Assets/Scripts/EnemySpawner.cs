using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] m_landEnemyPrefabs;
    public GameObject[] m_airEnemyPrefabs;

    public void SpawnEnemies(ChunkController chunkController)
    {
        SpawnEnemy(GetRandomPositionLand(chunkController.GetAndDeleteRandomLandSpawner()), GetRandomLandEnemy());
        SpawnEnemy(GetRandomPositionLand(chunkController.GetAndDeleteRandomLandSpawner()), GetRandomLandEnemy());
        SpawnEnemy(GetRandomPositionAir(chunkController.GetAndDeleteRandomAirSpawner()), GetRandomAirEnemy());
        SpawnEnemy(GetRandomPositionAir(chunkController.GetAndDeleteRandomAirSpawner()), GetRandomAirEnemy());
    }

    private Vector3 GetRandomPositionLand(BoxCollider2D spawner)
    {
        return new Vector3(Random.Range(spawner.transform.position.x - spawner.size.x * 0.5f, spawner.transform.position.x + spawner.size.x * 0.5f),
            spawner.transform.position.y - spawner.size.y * 0.5f,
            spawner.transform.position.z);
    }

    private Vector3 GetRandomPositionAir(BoxCollider2D spawner)
    {
        return new Vector3(Random.Range(spawner.transform.position.x - spawner.size.x * 0.5f, spawner.transform.position.x + spawner.size.x * 0.5f),
            Random.Range(spawner.transform.position.y - spawner.size.y * 0.5f, spawner.transform.position.y + spawner.size.y * 0.5f),
            spawner.transform.position.z);
    }

    private GameObject GetRandomLandEnemy()
    {
        return m_landEnemyPrefabs[Random.Range(0, m_landEnemyPrefabs.Length)];
    }

    private GameObject GetRandomAirEnemy()
    {
        return m_airEnemyPrefabs[Random.Range(0, m_airEnemyPrefabs.Length)];
    }

    private void SpawnEnemy(Vector3 position, GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity, this.transform);
    }
}
