using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositionComponent : MonoBehaviour
{
    public GameObject target = null;
    public float followSpeed = 5;

    void Update()
    {
        if (target)
        {
            Vector3 newPosition = transform.position;
            newPosition += (target.transform.position - transform.position) * Time.deltaTime * followSpeed;
            newPosition.z = -10;
            transform.SetPositionAndRotation(newPosition, Quaternion.identity);
        }
    }
}
