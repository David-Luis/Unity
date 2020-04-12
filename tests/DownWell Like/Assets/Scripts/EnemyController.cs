using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HealthController m_healthController;
    public bool canBeSmashed = true;

    private void Awake()
    {
        m_healthController = GetComponent<HealthController>();
    }

    void Update()
    {
        float distanceToCamera = transform.position.y - GameManager.systems.mainCamera.transform.position.y;
        if (distanceToCamera > GameManager.systems.mainCamera.orthographicSize * 1.5f)
        {
            Destroy(gameObject);
        }
    }

    public bool Hit(int damage)
    {
        m_healthController.RemoveLife(damage);
        if (m_healthController.GetLife() <= 0)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
