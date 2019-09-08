using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : MonoBehaviour
{
    public GameObject player;
    public const int RENDER_RADIUS = 1;

    public Material terrainMaterial;
    public const int CHUNK_SIZE = 16;
    public const int CHUNKS_COLUMN_HEIGHT = 16;
    public static Dictionary<string, Chunk> chunks;

    public Camera mainCamera;
    public Button playButton;
    public Slider sliderLoading;
    public Text textFPS;
    private float totalLoadingSteps;
    private int processCount;

    private bool firstBuild = true;
    private bool building = false;

    public static string GetChunkNameFromPosition(Vector3 position)
    {
        return (int)position.x + "_" + (int)position.y + "_" + (int)position.z;
    }

    /*void*/
    IEnumerator BuildWorld()
    {
        building = true;
        Vector3 playerChunkPosition = GetChunkPositionByWorldPosition(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        //IF CHUNK_RENDER_DISTANCE = 1, it is 9 * 2 (the last "*2" is because we first create the chunks and then we draw them)
        totalLoadingSteps = (Mathf.Pow(RENDER_RADIUS * 2 + 1, 2)) * CHUNKS_COLUMN_HEIGHT * 2;
        processCount = 0;

        for (int z = -RENDER_RADIUS; z <= RENDER_RADIUS; z++)
        {
            for (int x = -RENDER_RADIUS; x <= RENDER_RADIUS; x++)
            {
                for (int y = 0; y < CHUNKS_COLUMN_HEIGHT; y++)
                {
                    Vector3 chunkPosition = new Vector3((x + playerChunkPosition.x) * CHUNK_SIZE, y * CHUNK_SIZE, (z + playerChunkPosition.z) * CHUNK_SIZE);

                    Chunk chunk;
                    string chunkName = GetChunkNameFromPosition(chunkPosition);
                    if (chunks.TryGetValue(chunkName, out chunk))
                    {
                        chunk.status = Chunk.ChunkStatus.KEEP;
                        break;  //break because if a chunk in that column exists, all that column should be built
                                //small problem I see here: the other chunks are not marked as keep
                    }
                    else
                    {
                        chunk = new Chunk(chunkPosition, terrainMaterial);
                        chunk.chunkGameObject.transform.parent = transform;
                        chunks.Add(chunk.chunkGameObject.name, chunk);
                    }

                    if (firstBuild)
                    { 
                        IncreaseLoadingStep();
                    }
                    yield return null;
                }
            }
        }

        foreach (KeyValuePair<string, Chunk> chunk in chunks)
        {
            if (chunk.Value.status == Chunk.ChunkStatus.DRAW)
            {
                chunk.Value.DrawChunk();
                chunk.Value.status = Chunk.ChunkStatus.KEEP;
            }

            chunk.Value.status = Chunk.ChunkStatus.DONE;
            if (firstBuild)
            {
                IncreaseLoadingStep();
            }
            yield return null;
        }

        if (firstBuild)
        {
            OnEndFirstBuild();
        }

        building = false;
    }

    private void OnEndFirstBuild()
    {
        EnablePlayer();

        textFPS.gameObject.SetActive(true);
        sliderLoading.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        firstBuild = false;
    }

    private void IncreaseLoadingStep()
    {
        processCount++;
        sliderLoading.value = processCount / totalLoadingSteps * 100;
    }

    public void StartBuild()
    {
        playButton.gameObject.SetActive(false);
        sliderLoading.gameObject.SetActive(true);
        StartCoroutine(BuildWorld());
        //BuildWorld();
    }

    private void EnablePlayer()
    {
        //Moves down player until first solid block
        {
            int playerX = (int)Mathf.Floor(player.transform.position.x);
            int playerY = (int)Mathf.Floor(player.transform.position.y);
            int playerZ = (int)Mathf.Floor(player.transform.position.z);
            Block firstSolidBlock = null;
            while (playerY >= 0)
            {
                Vector3 playerChunkPosition = GetChunkPositionByWorldPosition(player.transform.position.x, playerY, player.transform.position.z);
                Chunk chunk = null;
                string chunkName = GetChunkNameFromPosition(playerChunkPosition * 16);
                chunks.TryGetValue(chunkName, out chunk);
                if (chunk != null)
                {
                    Block currentBlock = chunk.GetBlock(MathUtils.Mod(playerX, CHUNK_SIZE), MathUtils.Mod(playerY, CHUNK_SIZE), MathUtils.Mod(playerZ, CHUNK_SIZE));
                    if (currentBlock != null)
                    {
                        if (currentBlock.IsSolid())
                        {
                            firstSolidBlock = currentBlock;
                            break;
                        }
                    }
                }

                playerY--;
            }

            player.transform.position = new Vector3(playerX, playerY + 1, playerZ);
            player.SetActive(true);
        }
    }

    void Start()
    {
        player.SetActive(false);
        chunks = new Dictionary<string, Chunk>();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private Vector3 GetChunkPositionByWorldPosition(float x, float y, float z)
    {
        return new Vector3((int)Mathf.Floor(x / CHUNK_SIZE),
                           (int)Mathf.Floor(y / CHUNK_SIZE),
                           (int)Mathf.Floor(z / CHUNK_SIZE));
    }

    private void Update()
    {
        if (!building && !firstBuild)
        {
            StartCoroutine(BuildWorld());
        }
    }
}
