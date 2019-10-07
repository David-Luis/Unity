using TMPro;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    enum WeaponType
    {
        Normal = 0,
        Shootgun,
        MachineGun
    };

    public GameObject[] m_Weapons;

    private int m_currentWeapon;
    private float m_lastShootTime = 0;
    bool m_isAutomatic = false;
    private int m_maxAmmo;
    private int m_currentAmmo;

    public TextMeshProUGUI m_text;

    private void Awake()
    {
        SetWeapon((int)WeaponType.MachineGun);
    }

    public bool TryShootWeapon(Vector3 position)
    {
        Weapon weapon = Instantiate(m_Weapons[m_currentWeapon], position, Quaternion.identity).GetComponent<Weapon>();
        if (weapon.TryShoot(m_lastShootTime, m_currentAmmo))
        {
            m_isAutomatic = weapon.m_isAutomatic;
            m_lastShootTime = Time.realtimeSinceStartup;
            GameManager.systems.shakeController.ShakeShoot();
            m_currentAmmo--;
            UpdateText();
            return true;
        }

        return false;
    }

    public void SetWeapon(int idx)
    {
        CalculateAndSetMaxAmmo();
        Reload();
        m_currentWeapon = idx;
    }

    private void UpdateText()
    {
        if (m_text != null)
        {
            m_text.text = "Ammo: " + m_currentAmmo + "/" + m_maxAmmo;
        }
    }

    private void CalculateAndSetMaxAmmo()
    {
        Weapon weapon = Instantiate(m_Weapons[m_currentWeapon], Vector3.zero, Quaternion.identity).GetComponent<Weapon>();
        m_maxAmmo = weapon.m_baseAmmo;
        Destroy(weapon.gameObject);
    }

    public void Reload()
    {
        m_isAutomatic = false;
        m_currentAmmo = m_maxAmmo;
        UpdateText();
    }

    internal bool IsAutomatic()
    {
        return m_isAutomatic;
    }
}
