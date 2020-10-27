using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class HudController : IGameSystem
{
    private HudView hudView;

    public HudController()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    ~HudController()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;

        if (message.EventName == "onStartWave")
        {
            if (Systems.wavesController.CanStartWave())
            {
                Systems.wavesController.StartNextWave();
            }

            Refresh();
        }
        else if (message.EventName == "onPurchaseTurret1")
        {
            if (Systems.purchasesController.Purchase(Products.TURRET_1))
            {
                Systems.gameController.PlaceTurret(Products.TURRET_1);
            }
            Refresh();
        }
        else if (message.EventName == "onPurchaseTurret2")
        {
            if (Systems.purchasesController.Purchase(Products.TURRET_2))
            {
                Systems.gameController.PlaceTurret(Products.TURRET_2);
            }
            Refresh();
        }
        else if (message.EventName == "onShowHud")
        {
            GetView();
            Refresh();
        }
    }

    public void DisableWaveButton()
    {
        hudView.DisableWaveButton();
        Refresh();
    }

    public void EnableWaveButton()
    {
        hudView.EnableWaveButton();
        Refresh();
    }

    private void GetView()
    {
        hudView = GameObject.FindGameObjectWithTag("UIHud").GetComponent<HudView>();
    }

    public void Refresh()
    {
        hudView.Refresh();
    }

    public void Update(float deltaTime) { }
}
