using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
    public Camera camera;

    public GameObject[] chunkPrefabs;

    private int amountConcurrentChunks = 3;
    private float nextChunkY;
    private List<GameObject> chunks = new List<GameObject>();

    public void Start()
    {
        nextChunkY = 0;
        CreateInitialChunks();
    }

    private void CreateInitialChunks()
    {
        for (int i = 0; i < amountConcurrentChunks; i++)
        {
            AddChunk(CreateChunk(chunkPrefabs[0]));
        }
    }

    private GameObject CreateChunk(GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(0, nextChunkY, 0), Quaternion.identity);
    }

    private GameObject CreateRandomChunk()
    {
        return CreateChunk(chunkPrefabs[1]);
    }

    public void Update()
    {
        DeleteOutsideChunks();
        CreateNewChunksIfNeeded();
    }

    private void CreateNewChunksIfNeeded()
    {
        if (chunks.Count < amountConcurrentChunks)
        {
            AddChunk(CreateRandomChunk());
        }
    }

    private void AddChunk(GameObject chunk)
    {
        chunks.Add(chunk);
        nextChunkY -= GetChunkSize(chunk);
    }

    private void DeleteOutsideChunks()
    {
        for (int i = chunks.Count - 1; i >= 0; --i)
        {
            GameObject chunk = chunks[i];
            float distanceToCamera = chunk.transform.position.y - camera.transform.position.y;
            if (distanceToCamera > GetChunkSize(chunk))
            {
                Destroy(chunk);
                chunks.RemoveAt(i);
            }
        }
    }

    private float GetChunkSize(GameObject chunk)
    {
        return chunk.GetComponent<BoxCollider2D>().size.y;
    }
}
