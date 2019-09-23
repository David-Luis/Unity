using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeepWidth : MonoBehaviour
{
    public Vector2 referenceResolution;
    public float startSize;

    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: don't do this in the update
        float originalRatio = referenceResolution.y / referenceResolution.x;
        float currentRatio = (float)Screen.height / (float)Screen.width;
        float incrementRatio = currentRatio / originalRatio;

        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = startSize * incrementRatio;
    }
}
