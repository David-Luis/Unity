using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FollowHorizontalDeadzoneCamera2D : MonoBehaviour {

    public Renderer target;
    public Vector2 deadzone;
    public float verticalEasing = 0.04f;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update ()
    {
        float localX = target.transform.position.x - transform.position.x;

        Vector3 newPosition = _camera.transform.position;

        if (localX < deadzone.x)
        {
            newPosition.x += localX - deadzone.x;
        }
        else if (localX > deadzone.y)
        {
            newPosition.x += localX - deadzone.y;
        }

        float _currentVelocity = 0;
        newPosition.y = Mathf.SmoothDamp(newPosition.y, target.transform.position.y, ref _currentVelocity, verticalEasing);
        //newPosition.y = target.transform.position.y;

        _camera.transform.position = newPosition;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FollowHorizontalDeadzoneCamera2D))]
public class FollowHorizontalDeadzoneCamera2DEditor : Editor
{
    public void OnSceneGUI()
    {
        FollowHorizontalDeadzoneCamera2D cam = target as FollowHorizontalDeadzoneCamera2D;

        Vector3 p1Left = cam.transform.position + new Vector3(cam.deadzone.x, -1000, 0);
        Vector3 p2Left = cam.transform.position + new Vector3(cam.deadzone.x, 1000, 0);

        Vector3 p1Right = cam.transform.position + new Vector3(cam.deadzone.y, -1000, 0);
        Vector3 p2Right = cam.transform.position + new Vector3(cam.deadzone.y, 1000, 0);

        Handles.DrawLine(p1Left, p2Left);
        Handles.DrawLine(p1Right, p2Right);
    }
}
#endif
