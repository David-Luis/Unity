using System;
using UnityEngine;


public class CameraFollowVertical : MonoBehaviour
{
    //TODO: remove unnecesary comments

    public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
    public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
    public float minY = -5; // The minimum x and y coordinates the camera can have.
    public float maxY = 5; // The maximum x and y coordinates the camera can have.
    public float targetDelta = 0;

    public GameObject target; // Reference to the player's transform.

    private float GetTargetY()
    {
        return target.transform.position.y + targetDelta;
    }

    private bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - GetTargetY()) > yMargin;
    }

    private bool CheckYLimit(float targetY)
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return (GetTargetY() + targetY) >= maxY || (GetTargetY() + targetY) <= minY;
    }

    private void Update()
    {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        float targetY = transform.position.y;

        if (CheckYMargin())
        {
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, GetTargetY(), ySmooth * Time.deltaTime);
        }

        if (CheckYLimit(targetY))
        {
            targetY = Mathf.Clamp(targetY, GetTargetY() + minY, target.transform.position.y + maxY);
        }

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
