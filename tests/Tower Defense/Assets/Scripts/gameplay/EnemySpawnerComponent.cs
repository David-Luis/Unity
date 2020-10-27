using UnityEngine;

public class EnemySpawnerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy = null;
    [SerializeField]
    private GameObject playerBase = null;

    [SerializeField]
    private WaveDefinition[] waves = null;

    private WaveDefinition currentWave = null;
    private int currentWaveIndex;
    int increaseHelthEnemy = 0;

    int nextSpawnAmount = 0;
    float waveTotalTimeSeconds = 0;
    float nextSpawnTimeSeconds = 0;
    public bool IsWaveActive { get; private set; } = false;

    void Start()
    {
        currentWaveIndex = -1;
    }

    public void StartWave()
    {
        currentWave = GetNextWave();
        PrepareNextSpawn();
        nextSpawnTimeSeconds = 0;
        IsWaveActive = true;
        increaseHelthEnemy = currentWave.IncreaseHealthEnemy;
    }

    void Update()
    {
        if (IsWaveActive)
        {
            waveTotalTimeSeconds += Time.deltaTime;
            if (waveTotalTimeSeconds >= currentWave.DurationSeconds)
            {
                FinishWave();
            }
            else
            {
                ProcessCurrentWave();
            }
        }
    }

    private void PrepareNextSpawn()
    {
        nextSpawnAmount = Random.Range(currentWave.MinSpawnSimultaniousEnemies, currentWave.MaxSpawnSimultaniousEnemies+1);
        nextSpawnTimeSeconds = waveTotalTimeSeconds + Random.Range(currentWave.MinNextSpawnInSeconds, currentWave.MaxNextSpawnInSeconds);
    }

    private WaveDefinition GetNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex >= waves.Length)
        {
            currentWaveIndex = waves.Length-1;
        }

        return waves[currentWaveIndex];
    }

    private void ProcessCurrentWave()
    {
        if (waveTotalTimeSeconds >= nextSpawnTimeSeconds)
        {
            SpawnEnemies();
            PrepareNextSpawn();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < nextSpawnAmount; i++)
        {
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity, transform);
            EnemyComponent enemyComponent = newEnemy.GetComponent<EnemyComponent>();
            enemyComponent.AddExtraHealth(increaseHelthEnemy);
            enemyComponent.SetDestination(playerBase.transform);
        }
    }

    private void FinishWave()
    {
        IsWaveActive = false;
    }
}
