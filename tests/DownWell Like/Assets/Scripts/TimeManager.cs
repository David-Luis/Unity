using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float m_currentTimeScale = 1;

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = m_currentTimeScale;
    }

    public void SlowHitByEnemy()
    {
        m_currentTimeScale = 0.4f;
        Time.timeScale = m_currentTimeScale;
    }

    public void DisableSlowHitByEnemy()
    {
        m_currentTimeScale = 1.0f;
        Time.timeScale = m_currentTimeScale;
    }
}
