using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void Log(object message)
    {
        if (Debug.isDebugBuild)
        {
            Debug.Log(message);
        }
    }

    public static void Error(object message)
    {
        if (Debug.isDebugBuild)
        {
            Debug.LogError(message);
        }
    }
}
