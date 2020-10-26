using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class HudController : IGameSystem
{
    Button buttonWave = null;
    TextMeshProUGUI coinsText = null;

    const int TURRETS_AMOUNT = 3;
    List<GameObject> turretGameObjects;

    string[] turretsProducts = { Products.TURRET_1, Products.TURRET_2, Products.TURRET_3 };

    public HudController()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    ~HudController()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    public void EnableWaveButton()
    {
        buttonWave.interactable = true;
    }

    public void DisableWaveButton()
    {
        buttonWave.interactable = false;
    }


    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;

        if (message.EventName == "onStartWave")
        {
            if (Systems.wavesController.CanStartWave())
            {
                Systems.wavesController.StartWave();
            }
        }
        else if (message.EventName == "onPurchaseTurret1")
        {
            if (Systems.purchasesController.Purchase(turretsProducts[0]))
            {
                Systems.gameController.PlaceTurret(turretsProducts[0]);
            }
        }
        else if (message.EventName == "onShowHud")
        {
            CacheHudElements();
            Refresh();
        }
    }

    private void CacheHudElements()
    {
        GameObject hudGameObject = GameObject.FindGameObjectWithTag("UIHud");

        buttonWave = hudGameObject.transform.Find("Button - StartWave").GetComponent<Button>();
        coinsText = hudGameObject.transform.Find("coinsPlayer").GetComponent<TextMeshProUGUI>();

        turretGameObjects = new List<GameObject>(TURRETS_AMOUNT);
        for (int i = 0; i < TURRETS_AMOUNT; i++)
        {
            turretGameObjects.Add(hudGameObject.transform.Find("Turret_" + i.ToString()).gameObject);
        }

        ConfigureTurretsInfo();
    }

    private void ConfigureTurretsInfo()
    {
        for (int i = 0; i < TURRETS_AMOUNT; i++)
        {
            int value = Systems.purchasesController.GetProductValue(turretsProducts[i]);
            TextMeshProUGUI coinsText = turretGameObjects[i].transform.Find("coins").GetComponent<TextMeshProUGUI>();
            Button button = turretGameObjects[i].transform.Find("Button - Turret").GetComponent<Button>();

            coinsText.SetText(value.ToString());
            button.interactable = value <= Systems.currencyModel.GetCoins();
        }
    }

    public void Refresh()
    {
        coinsText.SetText(Systems.currencyModel.GetCoins().ToString());
    }

    public void Update(float deltaTime) { }
}
