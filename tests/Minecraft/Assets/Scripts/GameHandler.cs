using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public GameObject sand;
    public GameObject water;
    public GameObject earth;

    void Start()
    {
        //CreateSand();
        //CreateWater();
        CreateEarth();
    }

    private void CreateSand()
    {
        InstantiateVoxels(new Vector2Int(-10, 10), new Vector2Int(-10, -9), new Vector2Int(-10, 10), sand);
    }

    private void CreateWater()
    {
        InstantiateVoxels(new Vector2Int(-10, 10), new Vector2Int(-9, 0), new Vector2Int(-10, 10), water);
    }

    private void CreateEarth()
    {
        InstantiateVoxels(new Vector2Int(-50, 50), new Vector2Int(0, 1), new Vector2Int(-50, 50), earth);
    }

    private void InstantiateVoxels(Vector2Int horizontal, Vector2Int vertical, Vector2Int depth, GameObject gameObject)
    {
        int amountX = Math.Abs(horizontal.x - horizontal.y);
        int amountY = Math.Abs(vertical.x - vertical.y);
        int amountZ = Math.Abs(depth.x - depth.y);
        
        for (int i = 0; i < amountX; i++)
        {
            for (int j = 0; j < amountY; j++)
            {
                for (int k = 0; k < amountZ; k++)
                {
                    Instantiate(gameObject, new Vector3(horizontal.x + i, vertical.x + j, depth.x + k), Quaternion.identity);
                }
            }
        }
    }
}
