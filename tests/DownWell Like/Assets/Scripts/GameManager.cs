using DG.Tweening;
using Doozy.Engine;
using UnityEngine;

[RequireComponent(typeof(Systems))]
public class GameManager : MonoBehaviour
{
    public static Systems systems;

    // Start is called before the first frame update
    void Awake()
    {
        systems = GetComponent<Systems>();
        DOTween.Init();
    }

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
        if (message == null) return;

        if (message.EventName == "StartGame")
        {
            systems.player.gameObject.SetActive(true);
            systems.chunksManager.gameObject.SetActive(true);
        }
        else if (message.EventName == "PauseGame")
        {
            systems.timeManager.PauseGame();
        }
        else if (message.EventName == "ResumeGame")
        {
            systems.timeManager.ResumeGame();
        }

        //Debug.Log("UI EVENT: '" + message.EventName);
    }
}
