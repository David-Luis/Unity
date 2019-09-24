using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    public GameObject m_Weapon;

    public bool ShootWeapon(Vector3 position)
    {
        GameManager.systems.shakeController.ShakeShoot();
        Instantiate(m_Weapon, position, Quaternion.identity);

        return true;
    }
}
