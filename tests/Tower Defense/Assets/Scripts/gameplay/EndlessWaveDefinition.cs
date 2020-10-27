using UnityEngine;

public class EndlessWaveDefinition : MonoBehaviour
{
    public float IncreaseDurationSeconds = 1.0f;

    public float DecreaseMinNextSpawnInSeconds = 0.01f;

    [Range(1, 10)]
    public float DecreaseMaxNextSpawnInSeconds = 0.01f;

    public int IncreaseHealthEnemy = 10;
}