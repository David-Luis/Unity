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

    private List<GameObject> m_gameObjectsToProcessTurn;
    private List<GameObject> m_gameObjectsToDestruct;

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

        if (GameConfiguration.ShowDebugInformation)
        {
            m_player.AddComponent<DebugTextComponent>();
        }
    }

    private void AddEnemy(Vector2 position, GameObject enemyPredab)
    {
        GameObject enemy = Instantiate(enemyPredab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        enemy.GetComponent<EnemyComponent>().SetPlayer(m_player);
        m_dungeon.GetTileAtPosition(position).m_gameObjects.Add(enemy);
        m_gameObjectsToProcessTurn.Add(enemy);

        if (GameConfiguration.ShowDebugInformation)
        {
            enemy.AddComponent<DebugTextComponent>();
        }
    }

    private void InitGame()
    {
        m_gameObjectsToDestruct = new List<GameObject>();
        m_gameObjectsToProcessTurn = new List<GameObject>();

        m_dungeon = GetComponent<Dungeon>();
        m_dungeon.LoadFromCSV("Assets/Dungeons/test_dungeon.tsv");

        AddPlayer(new Vector2(1, -3));
        AddEnemy(new Vector2(4, -3), enemy1Prefab);
        AddEnemy(new Vector2(8, -5), enemy2Prefab);

        Camera.main.GetComponent<FollowPositionComponent>().target = m_player;
    }

    public static void MarkAsDestroy(GameObject gameObject)
    {
        instance.m_gameObjectsToDestruct.Add(gameObject);
    }

    public static void ProcessTurn()
    {
        foreach (var gameObject in instance.m_gameObjectsToDestruct)
        {
            Tile tile = instance.m_dungeon.GetTileAtPosition(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
            tile.m_gameObjects.Remove(gameObject);
            instance.m_gameObjectsToProcessTurn.Remove(gameObject);
            Destroy(gameObject);
        }
        instance.m_gameObjectsToDestruct.Clear();

        foreach (var gameObject in instance.m_gameObjectsToProcessTurn)
        {
            var components = gameObject.GetComponents<BaseComponent>();
            foreach (var component in components)
            {
                component.ProccessTurn();
            }
        }
    }

    public static Dungeon GetDungeon()
    {
        return instance.m_dungeon;
    }
}
