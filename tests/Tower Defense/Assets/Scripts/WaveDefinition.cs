using UnityEngine;

public class WaveDefinition : MonoBehaviour
{
    [SerializeField]
    public float DurationSeconds = 10.0f;

    [SerializeField]
    [Range(1, 4)]
    public int MinSpawnSimultaniousEnemies = 1;
    [SerializeField]
    [Range(1, 4)]
    public int MaxSpawnSimultaniousEnemies = 2;

    [SerializeField]
    [Range(1, 10)]
    public float MinNextSpawnInSeconds = 2;
    [SerializeField]
    [Range(1, 10)]
    public float MaxNextSpawnInSeconds = 3;

    //TODO: use a list of enemy and weights
    [SerializeField]
    private GameObject enemy;
}
