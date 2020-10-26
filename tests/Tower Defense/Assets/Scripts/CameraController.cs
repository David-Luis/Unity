﻿using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 10f;
    public float panBorderThickness = 50f;

    public float minY = 5f;
    public float maxY = 20f;

    public float zoomSpeed = 20f;
    private float zoomTargetY = 0;
    private float zoomSmooth = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        zoomTargetY = transform.position.y;
    }

    void Update()
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness && Input.mousePosition.y <= Screen.height)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness && Input.mousePosition.y >= 0)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness && Input.mousePosition.x <= Screen.width)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness && Input.mousePosition.x >= 1)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            Vector3 pos = transform.position;
            zoomTargetY = pos.y - scroll * zoomSpeed;
            zoomTargetY = Mathf.Clamp(zoomTargetY, minY, maxY);
        }

        
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, zoomTargetY, transform.position.z), ref velocity, zoomSmooth);
    }
}