using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretPrefabs = null;

    private void Start()
    {
        Systems.Init(this);
    }

    internal void OnPlayerDead()
    {
        //TODO: show game over
    }

    private void Update()
    {
        Systems.Update(Time.deltaTime);
    }

    public void PlaceTurret(string productId)
    {
        GameObject turret = Instantiate(turretPrefabs[0], Vector3.zero, Quaternion.identity);
        PlaceableComponent placeableComponent = turret.GetComponent<PlaceableComponent>();
        placeableComponent.StartPlacing();
    }

    internal void OnEnemyDead(int coinsReward)
    {
        Systems.currencyModel.AddCoins(coinsReward);
        Systems.hudController.Refresh();
    }
}
