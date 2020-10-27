using UnityEngine;

public class EnemySpawnerComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy = null;
    [SerializeField]
    private GameObject playerBase = null;

    [SerializeField]
    private WaveDefinition[] waves = null;

    [SerializeField]
    private float randomDeltaStartPosition = 1;

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
            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-randomDeltaStartPosition, randomDeltaStartPosition),
                                                transform.position.y,
                                                transform.position.z + Random.Range(-randomDeltaStartPosition, randomDeltaStartPosition));

            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity, transform);
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
