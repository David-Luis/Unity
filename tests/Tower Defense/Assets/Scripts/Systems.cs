using System;
using System.Collections.Generic;
using UnityEngine;

public class Systems : MonoBehaviour
{
    public static GameController gameController;

    public static HudController hudController;
    public static WavesController wavesController;
    public static ICurrencyModel currencyModel;
    public static PurchasesController purchasesController;

    private static List<IGameSystem> gameSystems;

    public static void Init(GameController _gameController)
    {
        gameSystems = new List<IGameSystem>();

        gameController = _gameController;

        hudController = new HudController();
        wavesController = new WavesController();
        currencyModel = new DebugCurrencyModel(30);
        purchasesController = new PurchasesController();

        gameSystems.Add(hudController);
        gameSystems.Add(wavesController);
        gameSystems.Add(currencyModel);
        gameSystems.Add(purchasesController);
    }

    public static void Update(float deltaTime)
    {
        foreach (IGameSystem gameSystem in gameSystems)
        {
            gameSystem.Update(deltaTime);
        }
    }
}
