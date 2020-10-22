using System;
using UnityEngine;

public class TargetLookAtComponent : MonoBehaviour
{
    private Transform target = null;

    [SerializeField]
    private float defaultRotationX = 0;
    [SerializeField]
    private float defaultRotationY = 0;
    [SerializeField]
    private float defaultRotationZ = 0;

    [SerializeField]
    private float turnSpeed = 10;

    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            RotateToTarget(lookRotation);
        }
        else
        {
            RotateToTarget(Quaternion.Euler(defaultRotationX, defaultRotationY, defaultRotationZ));
        }
    }

    private void RotateToTarget(Quaternion lookRotation)
    {
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void LookAt(Transform target)
    {
        this.target = target;
    }
}
