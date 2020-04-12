using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject playerPrefab;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;

    private GameObject m_player;
    private Dungeon m_dungeon;
    private List<GameObject> m_objectsToDestruct;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    private void AddPlayer(Vector2 position)
    {
        m_player = Instantiate(playerPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        m_dungeon.GetTileAtPosition(position).m_gameObjects.Add(m_player);
    }

    private void AddEnemy(Vector2 position, GameObject enemyPredab)
    {
        GameObject enemy = Instantiate(enemyPredab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        m_dungeon.GetTileAtPosition(position).m_gameObjects.Add(enemy);
    }

    private void InitGame()
    {
        m_objectsToDestruct = new List<GameObject>();

        m_dungeon = GetComponent<Dungeon>();
        m_dungeon.LoadFromCSV("Assets/Dungeons/test_dungeon.tsv");

        AddPlayer(new Vector2(1, -3));
        AddEnemy(new Vector2(3, -6), enemy1Prefab);

        Camera.main.GetComponent<FollowPositionComponent>().target = m_player;
    }

    public static void MarkAsDestroy(GameObject gameObject)
    {
        instance.m_objectsToDestruct.Add(gameObject);
    }

    public static void ProcessTurn()
    {
        foreach (var gameObject in instance.m_objectsToDestruct)
        {
            Tile tile = instance.m_dungeon.GetTileAtPosition(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
            tile.m_gameObjects.Remove(gameObject);
            Destroy(gameObject);
        }

        instance.m_objectsToDestruct.Clear();
    }

    public static Dungeon GetDungeon()
    {
        return instance.m_dungeon;
    }
}
