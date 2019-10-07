using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float m_shootCadence = 1.0f;
    public bool m_isAutomatic = false;
    public int m_baseAmmo = 10;

    public bool TryShoot(float lastShootTime, int currentAmmo)
    {
        if (currentAmmo > 0 && Time.realtimeSinceStartup >= (lastShootTime + m_shootCadence))
        {
            Shoot();
            return true;
        }

        Destroy(gameObject);
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
