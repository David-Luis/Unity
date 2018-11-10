using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int currentLevel = 1;
    public int higherLevel = 2;

    public int score = 0;
    public int highScore = 0;

    public static GameController instance;

    HUDController hudController;

    void Awake ()
    {
		if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        instance.hudController = FindObjectOfType<HUDController>();

        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        hudController.UpdateHUD();
    }

    public void ResetGame()
    {
        score = 0;
        currentLevel = 1;
        SceneManager.LoadScene("Level1");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void IncreaseLevel()
    {
        if (currentLevel < higherLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
        }

        SceneManager.LoadScene("Level" + currentLevel);
        hudController.UpdateHUD();
    }
}
