using Doozy.Engine.Nody;
using UnityEngine;

public class Systems : MonoBehaviour
{
    public Camera mainCamera;
    public ShakeController shakeController;
    public EnemySpawner enemySpawner;
    public Transform player;
    public ChunksManager chunksManager;
    public TimeManager timeManager;
    public HitOverlayController hitOverlayController;
    public GraphController graphController;

    public GameObject m_particleBoxes;
    public GameObject m_particleEnemies;
}
