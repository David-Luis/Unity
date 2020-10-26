using System;
using UnityEngine;

public class Systems : MonoBehaviour
{
    public static GameController gameController;
    public static HudController hudController;
    public static WavesController wavesController;

    public static void Init(GameController _gameController)
    {
        gameController = _gameController;
        hudController = new HudController();
        wavesController = new WavesController();
    }

    public static void Update(float deltaTime)
    {
        
    }
}
