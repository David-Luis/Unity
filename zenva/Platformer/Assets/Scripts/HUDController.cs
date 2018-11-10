using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

    public Text scoreLabel;

    private void Start()
    {
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (scoreLabel)
        {
            scoreLabel.text = "Score: " + GameController.instance.score;
        }
    }
}
