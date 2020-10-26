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

    private void Update()
    {
        Systems.Update(Time.deltaTime);
    }

    public void PlaceTurret()
    {
        GameObject turret = Instantiate(turretPrefabs[0], Vector3.zero, Quaternion.identity);
        PlaceableComponent placeableComponent = turret.GetComponent<PlaceableComponent>();
        placeableComponent.StartPlacing();
    }
}
