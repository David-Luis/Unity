using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FollowDeadzoneCamera2D : MonoBehaviour {

    public Renderer target;
    public Rect deadzone;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update () {

        float localX = target.transform.position.x - transform.position.x;
        float localY = target.transform.position.y - transform.position.y;

        Vector3 newPosition = _camera.transform.position;

        if (localX < deadzone.xMin)
        {
            newPosition.x += localX - deadzone.xMin;
        }
        else if (localX > deadzone.xMax)
        {
            newPosition.x += localX - deadzone.xMax;
        }

        if (localY < deadzone.yMin)
        {
            newPosition.y += localY - deadzone.yMin;
        }
        else if (localY > deadzone.yMax)
        {
            newPosition.y += localY - deadzone.yMax;
        }

        _camera.transform.position = newPosition;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FollowDeadzoneCamera2D))]
public class FollowDeadzoneCamera2DEditor : Editor
{
    public void OnSceneGUI()
    {
        FollowDeadzoneCamera2D cam = target as FollowDeadzoneCamera2D;

        Vector3[] vert =
        {
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMax, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMax, 0)
        };

        Color transp = new Color(0, 0, 0, 0);
        Handles.DrawSolidRectangleWithOutline(vert, transp, Color.red);
    }
}
#endif
