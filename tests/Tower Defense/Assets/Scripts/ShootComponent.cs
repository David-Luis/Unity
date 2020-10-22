using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootComponent : MonoBehaviour
{
    [SerializeField]
    private Transform shootPosition = null;

    [SerializeField]
    private GameObject bullet = null;

    private Transform target = null;

    public void Shoot(Transform target)
    {
        //TODO:create shoot
        this.target = target;
    }
}
