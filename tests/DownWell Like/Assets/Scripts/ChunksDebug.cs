using System.Collections.Generic;
using UnityEngine;

public class ChunksDebug : MonoBehaviour
{
    public GameObject m_chunkDebug;
   
    private static uint ID = 0;

    public EnemySpawnerDebug enemySpawnerDebug;

    public void Start()
    {
        CreateDebugChunks();
    }

    private void CreateDebugChunks()
    {
        CreateChunk(m_chunkDebug);
    }

    private GameObject CreateChunk(GameObject prefab)
    {
        GameObject chunk = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        chunk.GetComponent<ChunkDebug>().enemySpawnerDebug = enemySpawnerDebug;

        chunk.name += "_" + (ID++);
        return chunk;
    }
}
