using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public enum ChunkStatus { DRAW, DONE, KEEP };
    public ChunkStatus status;

    public Material blockMaterial;
    public Block[,,] chunkData;
    public GameObject chunkGameObject;

    void BuildChunk()
    {
        chunkData = new Block[World.CHUNK_SIZE, World.CHUNK_SIZE, World.CHUNK_SIZE];

        for (int z = 0; z < World.CHUNK_SIZE; z++)
        {
            for (int y = 0; y < World.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < World.CHUNK_SIZE; x++)
                {
                    Vector3 position = new Vector3(x, y, z);
                    Vector3 worldPosition = new Vector3((int)(x + chunkGameObject.transform.position.x),
                                                        (int)(y + chunkGameObject.transform.position.y),
                                                        (int)(z + chunkGameObject.transform.position.z));

                    int worldHeigh = TerrainUtils.GenerateHeight(worldPosition.x, worldPosition.z);
                    int stoneHeigh = TerrainUtils.GenerateStoneHeight(worldPosition.x, worldPosition.z);
                    bool isCave = TerrainUtils.IsCave(worldPosition.x, worldPosition.y, worldPosition.z);
                    bool isLastLayer = worldPosition.y == 0;

                    Block.BlockType blockType = Block.BlockType.AIR;

                    if (isLastLayer)
                    {
                        blockType = Block.BlockType.BEDROCK;
                    }
                    else if (isCave)
                    {
                        blockType = Block.BlockType.AIR;
                    }
                    else if (worldPosition.y <= stoneHeigh)
                    {
                        bool isRedstone = TerrainUtils.IsRedstone(worldPosition.x, worldPosition.y, worldPosition.z);
                        bool isDiamond = TerrainUtils.IsDiamond(worldPosition.x, worldPosition.y, worldPosition.z);
                        if (isRedstone)
                        {
                            //Debug.Log("IS REDSTONE: " + worldPosition);
                            blockType = Block.BlockType.REDSTONE;
                        }
                        else if (isDiamond)
                        {
                            //Debug.Log("IS DIAMOND: " + worldPosition);
                            blockType = Block.BlockType.DIAMOND;
                        }
                        else
                        {
                            blockType = Block.BlockType.STONE;
                        }
                    }
                    else if (worldPosition.y == worldHeigh)
                    {
                        blockType = Block.BlockType.GRASS;
                    }
                    else if (worldPosition.y < worldHeigh)
                    {
                        blockType = Block.BlockType.DIRT;
                    }

                    chunkData[x, y, z] = new Block(blockType, position, chunkGameObject, this);

                    status = ChunkStatus.DRAW;
                }
            }
        }
    }

    public void DrawChunk()
    {
        for (int z = 0; z < World.CHUNK_SIZE; z++)
        {
            for (int y = 0; y < World.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < World.CHUNK_SIZE; x++)
                {
                    chunkData[x, y, z].Draw();
                }
            }
        }
        CombineQuads();
        MeshCollider collider = chunkGameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = chunkGameObject.transform.GetComponent<MeshFilter>().mesh;
    }

    private void CombineQuads()
    {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = chunkGameObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter meshFilter = chunkGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();

        //3. Add combined meshes on choldren as the parent's mesh
        meshFilter.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = chunkGameObject.AddComponent<MeshRenderer>();
        renderer.material = blockMaterial;

        //5. Delete all uncombined children
        foreach (Transform quad in chunkGameObject.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }
    }

    public Chunk(Vector3 position, Material blockMaterial)
    {
        chunkGameObject = new GameObject(World.GetChunkNameFromPosition(position));
        chunkGameObject.transform.position = position;
        this.blockMaterial = blockMaterial;
        BuildChunk();
    }

    public Block GetBlock(int x, int y, int z)
    {
        bool isOutsideChunk = x >= World.CHUNK_SIZE || x < 0 || y >= World.CHUNK_SIZE || y < 0 || z >= World.CHUNK_SIZE || z < 0;
        if (isOutsideChunk)
        {
            Vector3 neighbourChunkPos = chunkGameObject.transform.position +
               GetChunkDeltaPositionForBlockCoordinates(x, y, z);

            string nameNeighbourChunk = World.GetChunkNameFromPosition(neighbourChunkPos);

            x = ConvertBlockCoordinateToLocal(x);
            y = ConvertBlockCoordinateToLocal(y);
            z = ConvertBlockCoordinateToLocal(z);

            Chunk neighbourChunk;
            if (World.chunks.TryGetValue(nameNeighbourChunk, out neighbourChunk))
            {
                return neighbourChunk.chunkData[x, y, z];
            }

            return null;
        }

        return chunkData[x, y, z];
    }

    private Vector3 GetChunkDeltaPositionForBlockCoordinates(int x, int y, int z)
    {
        return new Vector3(x < 0 || x >= World.CHUNK_SIZE ? x * World.CHUNK_SIZE : 0,
                           y < 0 || y >= World.CHUNK_SIZE ? y * World.CHUNK_SIZE : 0,
                           z < 0 || z >= World.CHUNK_SIZE ? z * World.CHUNK_SIZE : 0);

    }

    private int ConvertBlockCoordinateToLocal(int coordinate)
    {
        if (coordinate == -1)
        {
            return World.CHUNK_SIZE - 1;
        }
        if (coordinate == World.CHUNK_SIZE)
        {
            return 0;
        }

        return coordinate;
    }
}
