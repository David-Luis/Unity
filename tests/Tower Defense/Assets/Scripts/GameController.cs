using Doozy.Engine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretPrefabs = null;

    private Dictionary<string, int> turretIndexPerProduct = new Dictionary<string, int>() {
            { Products.TURRET_1, 0 },
            { Products.TURRET_2, 1 },
            { Products.TURRET_3, 2 }
        };

    private void Start()
    {
        Debug.Log("SYSTEMS INIT");
        Systems.Init(this);

        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        Systems.Destroy();
    }

    private void OnEnable()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message == null) return;

        if (message.EventName == "onPause")
        {
            PauseGame();
        }
        else if (message.EventName == "onResume")
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void OnPlayerDead()
    {
        PauseGame();
        GameEventMessage.SendEvent("OnPlayerDead");
    }

    public void OnPause()
    {
    }

    private void Update()
    {
        Systems.Update(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Systems.currencyModel.AddCoins(10);
            Systems.hudController.Refresh();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            OnPlayerDead();
        }
    }

    public void PlaceTurret(string productId)
    {
        GameObject turret = Instantiate(turretPrefabs[turretIndexPerProduct[productId]], Vector3.zero, Quaternion.identity);
        PlaceableComponent placeableComponent = turret.GetComponent<PlaceableComponent>();
        placeableComponent.StartPlacing();
    }

    internal void OnEnemyDead(int coinsReward)
    {
        Systems.currencyModel.AddCoins(coinsReward);
        Systems.hudController.Refresh();
    }
}
