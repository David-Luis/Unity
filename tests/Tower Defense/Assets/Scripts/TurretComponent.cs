using UnityEngine;

public class TurretComponent : MonoBehaviour
{
    [SerializeField]
    private GameObject rangeCilinder = null;

    [SerializeField]
    private ShootComponent shootComponent = null;

    [SerializeField]
    private TargetLookAtComponent targetLookAtComponent = null;

    [SerializeField]
    private float minRange = 10;

    [SerializeField]
    private float maxRange = 100;

    private Transform target = null;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        target = null;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= maxRange && distanceToEnemy >= minRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }

        targetLookAtComponent.LookAt(target);
    }
}
