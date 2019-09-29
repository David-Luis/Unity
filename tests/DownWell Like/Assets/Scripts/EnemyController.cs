using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    void Update()
    {
        float distanceToCamera = transform.position.y - GameManager.systems.mainCamera.transform.position.y;
        if (distanceToCamera > GameManager.systems.mainCamera.orthographicSize * 1.5f)
        {
            Destroy(gameObject);
        }
    }
}
