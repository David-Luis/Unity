using UnityEngine;
using Doozy.Engine;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
                Debug.Log("Sending: OpenCheats");
                GameEventMessage.Send("OpenCheats");
            }
        }
    }
}
