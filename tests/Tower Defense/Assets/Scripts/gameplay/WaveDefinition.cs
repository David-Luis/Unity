using UnityEngine;

public class WaveDefinition : MonoBehaviour
{
    public float DurationSeconds = 10.0f;

    [Range(1, 4)]
    public int MinSpawnSimultaniousEnemies = 1;

    [Range(1, 4)]
    public int MaxSpawnSimultaniousEnemies = 2;


    [Range(1, 10)]
    public float MinNextSpawnInSeconds = 2;

    [Range(1, 10)]
    public float MaxNextSpawnInSeconds = 3;

    public int IncreaseHealthEnemy = 20;

    //TODO: use a list of enemy and weights
    [SerializeField]
    private GameObject enemy;
}
