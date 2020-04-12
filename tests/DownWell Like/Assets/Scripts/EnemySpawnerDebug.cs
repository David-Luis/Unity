using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerDebug : MonoBehaviour
{
    public GameObject m_enemyPrefab;
    public bool m_isLand = true;

    public void SpawnEnemies(ChunkDebug chunkController)
    {
        if (m_isLand)
        {
            SpawnEnemy(GetRandomPositionLand(chunkController.GetRandomLandSpawner()), m_enemyPrefab);
        }
        else
        {
            SpawnEnemy(GetRandomPositionAir(chunkController.GetRandomAirSpawner()), m_enemyPrefab);
        }
    }

    private Vector3 GetRandomPositionLand(BoxCollider2D spawner)
    {
        return new Vector3(Random.Range(spawner.transform.position.x - spawner.size.x * 0.5f, spawner.transform.position.x + spawner.size.x * 0.5f),
            spawner.transform.position.y - spawner.size.y * 0.5f,
            spawner.transform.position.z);
    }

    private Vector3 GetRandomPositionAir(BoxCollider2D spawner)
    {
       

        Vector3 position = new Vector3(Random.Range(spawner.transform.position.x - spawner.size.x * 0.5f, spawner.transform.position.x + spawner.size.x * 0.5f),
            Random.Range(spawner.transform.position.y - spawner.size.y * 0.5f, spawner.transform.position.y + spawner.size.y * 0.5f),
            spawner.transform.position.z);

        Logger.Log(position);

        return position;
    }

    private void SpawnEnemy(Vector3 position, GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity, this.transform);
    }
}
