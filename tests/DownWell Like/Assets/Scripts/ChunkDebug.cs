using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkDebug : MonoBehaviour
{
    public List<BoxCollider2D> landSpawners = new List<BoxCollider2D>();
    public List<BoxCollider2D> airSpawners = new List<BoxCollider2D>();

    public EnemySpawnerDebug enemySpawnerDebug;

    private void Awake()
    {
        Transform spawners = transform.Find("spawners");
        Assert.IsNotNull(spawners);

        Transform landSpawnersParent = spawners.Find("land_spawners");
        Transform airSpawnersParent = spawners.Find("air_spawners");

        if (landSpawnersParent)
        {
            foreach (Transform child in landSpawnersParent)
            {
                landSpawners.Add(child.GetComponent<BoxCollider2D>());
            }
        }
       
        if (airSpawnersParent)
        {
            foreach (Transform child in airSpawnersParent)
            {
                airSpawners.Add(child.GetComponent<BoxCollider2D>());
            }
        }
    }

    private void Start()
    {
        enemySpawnerDebug.SpawnEnemies(this);
    }

    public BoxCollider2D GetRandomLandSpawner()
    {
        return GetRandomSpawner(landSpawners);
    }

    public BoxCollider2D GetRandomAirSpawner()
    {
        return GetRandomSpawner(airSpawners);
    }

    private BoxCollider2D GetRandomSpawner(List<BoxCollider2D> spawners)
    {
        int randomIndex = Random.Range(0, spawners.Count - 1);
        BoxCollider2D randomSpawner = spawners[randomIndex];

        return randomSpawner;
    }

    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                enemySpawnerDebug.SpawnEnemies(this);
            }
        }
    }
}
