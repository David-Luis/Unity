using Doozy.Engine;
using UnityEngine;
using UnityEngine.UI;

public class HudController : IGameSystem
{
    public HudController()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    ~HudController()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    public void Update()
    {
        
    }

    public void EnableWaveButton()
    {
        GetWaveButton().interactable = true;
    }

    public void DisableWaveButton()
    {
        GetWaveButton().interactable = false;
    }

    private Button GetWaveButton()
    {
        GameObject hudGameObject = GameObject.FindGameObjectWithTag("UIHud");
        Transform buttonWave = hudGameObject.transform.Find("Button - StartWave");
        return buttonWave.GetComponent<Button>();
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
            Systems.gameController.PlaceTurret();
        }
    }
}
