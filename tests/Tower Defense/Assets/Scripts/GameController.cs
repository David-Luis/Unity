using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Systems.Init(this);
    }

    private void Update()
    {
        Systems.Update(Time.deltaTime);
    }

    public void PlaceTurrent()
    {

    }
}
