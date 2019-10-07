using UnityEngine;
using Doozy.Engine;

public class DebugManager : MonoBehaviour
{
    private void OnEnable()
    {
        //Start listening for game events
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable()
    {
        //Stop listening for game events
        Message.RemoveListener<GameEventMessage>(OnMessage);
    }

    private void OnMessage(GameEventMessage message)
    {
        if (message.EventName == "DebugSetWeaponNormal")
        {
            GameManager.systems.weaponShooter.SetWeapon(WeaponShooter.WeaponType.Normal);
        }
        else if (message.EventName == "DebugSetWeaponShootgun")
        {
            GameManager.systems.weaponShooter.SetWeapon(WeaponShooter.WeaponType.Shootgun);
        }
        else if (message.EventName == "DebugSetWeaponMachineGun")
        {
            GameManager.systems.weaponShooter.SetWeapon(WeaponShooter.WeaponType.MachineGun);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Mathf.Approximately(Time.timeScale, 1.0f))
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (GameManager.systems.graphController.Graph.ActiveNode.Name == "HUD")
            {
                GameEventMessage.SendEvent("OpenCheats");
            }
        }
    }
}
