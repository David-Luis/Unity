using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel : MonoBehaviour
{
    private const int WORLD_SIZE = 5000;
    private const int SEA_LEVEL_LAYEAR = 100;
    private const int MIN_LAYER = 0;
    private const int MAX_LAYER = 500;

    private VoxelType[,,] voxels;

    private enum VoxelType
    {
        EARTH,
        SAND,
        SEA,
        STONE
    }

    // Start is called before the first frame update
    void Start()
    {
        voxels = new VoxelType[WORLD_SIZE, MAX_LAYER, WORLD_SIZE];
        CreateFromSeaLevelToSeaBottom();   
    }

    private void CreateFromSeaLevelToSeaBottom()
    {
        
    }
}
