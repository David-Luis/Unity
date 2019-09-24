﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
    public Camera m_camera;
    public GameObject[] m_chunkPrefabs;

    private int mAmountConcurrentChunks = 3;
    private float mNextChunkY;
    private List<GameObject> mChunks = new List<GameObject>();

    public void Start()
    {
        mNextChunkY = 0;
        CreateInitialChunks();
    }

    private void CreateInitialChunks()
    {
        for (int i = 0; i < mAmountConcurrentChunks; i++)
        {
            AddChunk(CreateChunk(m_chunkPrefabs[0]));
        }
    }

    private GameObject CreateChunk(GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(0, mNextChunkY, 0), Quaternion.identity);
    }

    private GameObject CreateRandomChunk()
    {
        return CreateChunk(m_chunkPrefabs[2]);
    }

    public void Update()
    {
        DeleteOutsideChunks();
        CreateNewChunksIfNeeded();
    }

    private void CreateNewChunksIfNeeded()
    {
        if (mChunks.Count < mAmountConcurrentChunks)
        {
            AddChunk(CreateRandomChunk());
        }
    }

    private void AddChunk(GameObject chunk)
    {
        mChunks.Add(chunk);
        mNextChunkY -= GetChunkSize(chunk);
    }

    private void DeleteOutsideChunks()
    {
        for (int i = mChunks.Count - 1; i >= 0; --i)
        {
            GameObject chunk = mChunks[i];
            float distanceToCamera = chunk.transform.position.y - m_camera.transform.position.y;
            if (distanceToCamera > GetChunkSize(chunk))
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
