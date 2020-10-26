using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HudController : IGameSystem
{
    Button buttonWave = null;
    TextMeshProUGUI coinsText = null;

    const int TURRETS_AMOUNT = 3;
    List<TurretUIHudComponent> turretHudComponents;

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
            if (Systems.purchasesController.Purchase(Products.TURRET_1))
            {
                Systems.gameController.PlaceTurret(Products.TURRET_1);
            }
            Refresh();
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

        turretHudComponents = new List<TurretUIHudComponent>(TURRETS_AMOUNT);
        for (int i = 0; i < TURRETS_AMOUNT; i++)
        {
            turretHudComponents.Add(hudGameObject.transform.Find("Turret_" + i.ToString()).gameObject.GetComponent<TurretUIHudComponent>());
        }

        RefreshTurretsInfo();
    }

    private void RefreshTurretsInfo()
    {
        foreach (TurretUIHudComponent turretComponent in turretHudComponents)
        {
            turretComponent.Refresh();
        }
    }

    public void Refresh()
    {
        coinsText.SetText(Systems.currencyModel.GetCoins().ToString());
        RefreshTurretsInfo();
    }

    public void Update(float deltaTime) { }
}
