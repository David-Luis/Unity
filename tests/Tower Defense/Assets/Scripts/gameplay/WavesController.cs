using System;
using UnityEngine;

public class WavesController : IGameSystem
{
    private int currentWave = 0;
    private bool isWaveActive = false;

    GameObject[] currentWaveSpawners = null;
    GameObject[] portals = null;

    public WavesController()
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        DisablePortals();
    }

    public bool CanStartWave()
    {
        return !isWaveActive;
    }

    public void StartWave()
    {
        EnablePortals();

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

    private void EnablePortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(true);
        }
    }

    private void DisablePortals()
    {
        foreach (GameObject portal in portals)
        {
            portal.SetActive(false);
        }
    }

    public void Update(float dt)
    {
        if (isWaveActive)
        {
            bool isSpawining = false;
            foreach (GameObject gameObject in currentWaveSpawners)
            {
                EnemySpawnerComponent enemySpawnerComponent = gameObject.GetComponent<EnemySpawnerComponent>();
                if (enemySpawnerComponent && enemySpawnerComponent.IsWaveActive)
                {
                    isSpawining = true;
                    break;
                }
            }

            if (!isSpawining)
            {
                isWaveActive = false;
                DisablePortals();
            }
        }
    }
}
