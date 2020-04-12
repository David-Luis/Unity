using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChunkController : MonoBehaviour
{
    public List<BoxCollider2D> landSpawners = new List<BoxCollider2D>();
    public List<BoxCollider2D> airSpawners = new List<BoxCollider2D>();

    private void Awake()
    {
        Transform spawners = transform.Find("spawners");
        Assert.IsNotNull(spawners);

        Transform landSpawnersParent = spawners.Find("land_spawners");
        Assert.IsNotNull(landSpawnersParent);

        Transform airSpawnersParent = spawners.Find("air_spawners");
        Assert.IsNotNull(airSpawnersParent);

        foreach (Transform child in landSpawnersParent)
        {
            landSpawners.Add(child.GetComponent<BoxCollider2D>());
        }

        foreach (Transform child in airSpawnersParent)
        {
            airSpawners.Add(child.GetComponent<BoxCollider2D>());
        }
    }

    private void Start()
    {
        if (GameManager.systems != null)
        {
            GameManager.systems.enemySpawner.SpawnEnemies(this);
        }
    }

    public BoxCollider2D GetAndDeleteRandomLandSpawner()
    {
        return GetAndDeleteRandomSpawner(landSpawners);
    }

    public BoxCollider2D GetAndDeleteRandomAirSpawner()
    {
        return GetAndDeleteRandomSpawner(airSpawners);
    }

    private BoxCollider2D GetAndDeleteRandomSpawner(List<BoxCollider2D> spawners)
    {
        int randomIndex = Random.Range(0, spawners.Count - 1);
        BoxCollider2D randomSpawner = spawners[randomIndex];
        spawners.RemoveAt(randomIndex);

        return randomSpawner;
    }
}
