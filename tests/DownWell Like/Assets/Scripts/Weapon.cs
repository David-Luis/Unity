using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float m_shootCadence = 1.0f;

    public bool TryShoot(float lastShootTime)
    {
        if (Time.realtimeSinceStartup >= (lastShootTime + m_shootCadence))
        {
            Shoot();
            return true;
        }

        return false;
    }

    private void Shoot()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
