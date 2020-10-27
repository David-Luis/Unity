using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudView : MonoBehaviour
{
    [SerializeField]
    Button buttonWave = null;

    [SerializeField]
    TextMeshProUGUI coinsText = null;

    [SerializeField]
    ShakeScaleAnimationUI coinsAnimation = null;

    [SerializeField]
    TurretUIHudComponent[] turretHudComponents = null;

    [SerializeField]
    TextMeshProUGUI waveName = null;

    int currrentCoins = 0;

    public void Start()
    {
        Systems.hudController.OnShowHud();
    }

    public void EnableWaveButton()
    {
        buttonWave.interactable = true;
    }

    public void DisableWaveButton()
    {
        buttonWave.interactable = false;
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
        if (Systems.currencyModel.GetCoins() > currrentCoins)
        {
            PlayGainCoinsAnimation();
        }

        currrentCoins = Systems.currencyModel.GetCoins();
        coinsText.SetText(currrentCoins.ToString());
        waveName.SetText(Systems.wavesController.GetCurrentWaveName());

        RefreshTurretsInfo();
    }

    private void PlayGainCoinsAnimation()
    {
        coinsAnimation.Animate();
    }
}
