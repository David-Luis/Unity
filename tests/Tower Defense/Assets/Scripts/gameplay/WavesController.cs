using System;
using UnityEngine;

public class WavesController : IGameSystem
{
    private int currentWave = 0;
    private bool isWaveSpawining = false;
    private bool isWaveActive = false;

    GameObject[] currentWaveSpawners = null;
    GameObject[] portals = null;

    public WavesController()
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        DisablePortals();
    }

    public void Destroy()
    {
    }

    public bool CanStartWave()
    {
        return !isWaveSpawining && !isWaveActive;
    }

    public void StartNextWave()
    {
        EnablePortals();

        isWaveSpawining = true;
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

        Systems.hudController.StartWave();
    }

    public string GetCurrentWaveName()
    {
        if (CanStartWave())
        {
            return "Next -> Wave " + (currentWave+1);
        }
        else
        {
            return "Wave " + currentWave;
        }
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
        if (isWaveSpawining)
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
                isWaveSpawining = false;
                DisablePortals();
            }
        }
        else if (isWaveActive)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                isWaveActive = false;
                Systems.hudController.FinishWave();
            }
        }
    }
}
