using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour {

    public Text score;

    private void Start()
    {
        score.text = GameController.instance.score.ToString();
    }

    public void StartGame()
    {
        GameController.instance.ResetGame();
    }
}
