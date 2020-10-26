using UnityEngine;

public class WavesController : IGameSystem
{
    private int currentWave = 0;
    private bool isWaveActive = false;

    GameObject[] currentWaveSpawners = null;

    public bool CanStartWave()
    {
        return !isWaveActive;
    }

    public void StartWave()
    {
        isWaveActive = true;
        currentWave++;

        currentWaveSpawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject gameObject in currentWaveSpawners)
        {
            EnemySpawnerComponent enemySpawnerComponent = gameObject.GetComponent<EnemySpawnerComponent>();
            if (enemySpawnerComponent)
            {
                enemySpawnerComponent.StartWave();
            }
            else
            {
                Debug.LogError("Missing EnemySpawnerComponent in Spawner");
            }
        }

        Systems.hudController.DisableWaveButton();
    }

    public void Update(float dt)
    {
        
    }
}
