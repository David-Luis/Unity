using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretPrefabs = null;

    private Dictionary<string, int> turretIndexPerProduct = new Dictionary<string, int>() {
            { Products.TURRET_1, 2 },
            { Products.TURRET_2, 2 },
            { Products.TURRET_3, 2 }
        };

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
