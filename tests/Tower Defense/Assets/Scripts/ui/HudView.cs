using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [SerializeField]
    GameObject waveFinished = null;

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

    public void ShowEndWaveAnnouncement()
    {
        waveFinished.SetActive(true);
        waveFinished.transform.localScale = new Vector3(0, 0, 0);
        waveFinished.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            waveFinished.transform.DOScale(Vector3.zero, 0.25f).SetDelay(1.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                waveFinished.SetActive(false);
            });
        });
    }
}
