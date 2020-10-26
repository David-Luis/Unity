using System;
using UnityEngine;

public abstract class BulletComponent : MonoBehaviour
{
    public abstract void Shoot(Transform target);
}
