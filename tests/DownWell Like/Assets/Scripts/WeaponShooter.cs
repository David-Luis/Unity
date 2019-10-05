using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public GameObject m_Weapon;

    private float m_lastShootTime = 0;

    public bool TryShootWeapon(Vector3 position)
    {
        Weapon weapon = Instantiate(m_Weapon, position, Quaternion.identity).GetComponent<Weapon>();
        if (weapon.TryShoot(m_lastShootTime))
        {
            m_lastShootTime = Time.realtimeSinceStartup;
            GameManager.systems.shakeController.ShakeShoot();
            return true;
        }

        return false;
    }
}
