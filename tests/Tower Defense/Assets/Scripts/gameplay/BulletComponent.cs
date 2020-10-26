using System;
using UnityEngine;

public abstract class BulletComponent : MonoBehaviour
{
    [SerializeField]
    protected int damage = 100;

    public abstract void Shoot(Transform target);
}
