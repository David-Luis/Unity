using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
    public GameObject[] m_initialChunkPrefabs;
    public GameObject[] m_chunkPrefabs;
   
    private int m_amountConcurrentChunks = 4;
    private float m_nextChunkY;
    private List<GameObject> mChunks = new List<GameObject>();

    private static uint ID = 0;

    public void Start()
    {
        m_nextChunkY = 0;
        CreateInitialChunks();
    }

    private void CreateInitialChunks()
    {
        foreach (GameObject chunk in m_initialChunkPrefabs)
        {
            AddChunk(CreateChunk(chunk));
        }
    }

    private GameObject CreateChunk(GameObject prefab)
    {
        GameObject chunk = Instantiate(prefab, new Vector3(0, m_nextChunkY, 0), Quaternion.identity);
        chunk.name += "_" + (ID++);
        return chunk;
    }

    private GameObject CreateRandomChunk()
    {
        return CreateChunk(m_chunkPrefabs[Random.Range(0, m_chunkPrefabs.Length)]);
    }

    public void Update()
    {
        DeleteOutsideChunks();
        CreateNewChunksIfNeeded();
    }

    private void CreateNewChunksIfNeeded()
    {
        if (mChunks.Count < m_amountConcurrentChunks)
        {
            AddChunk(CreateRandomChunk());
        }
    }

    private void AddChunk(GameObject chunk)
    {
        mChunks.Add(chunk);
        m_nextChunkY -= GetChunkSize(chunk);
    }

    private void DeleteOutsideChunks()
    {
        for (int i = mChunks.Count - 1; i >= 0; --i)
        {
            GameObject chunk = mChunks[i];
            float distanceToCamera = chunk.transform.position.y - GameManager.systems.mainCamera.transform.position.y;

            //I did this to be sure, makes sense? without * 2 will be optimized
            if (distanceToCamera > GetChunkSize(chunk) * 2)
            {
                Destroy(chunk);
                mChunks.RemoveAt(i);
            }
        }
    }

    private float GetChunkSize(GameObject chunk)
    {
        return chunk.GetComponent<BoxCollider2D>().size.y;
    }
}
