using UnityEngine;

public class TurretComponent : MonoBehaviour, IPlaceableListener
{
    [SerializeField]
    private GameObject rangeCilinder = null;

    [SerializeField]
    private GameObject minRangeCilinder = null;

    [SerializeField]
    private ShootComponent shootComponent = null;

    [SerializeField]
    private TargetLookAtComponent targetLookAtComponent = null;

    [SerializeField]
    private float minRange = 10;

    [SerializeField]
    private float maxRange = 100;

    private Transform target = null;
    private bool isPlacing = false;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.2f);

        rangeCilinder.transform.localScale = new Vector3(maxRange*2, rangeCilinder.transform.localScale.y, maxRange*2);
        minRangeCilinder.transform.localScale = new Vector3(minRange*2, minRangeCilinder.transform.localScale.y, minRange*2);

        rangeCilinder.SetActive(false);
        minRangeCilinder.SetActive(false);
    }

    private void UpdateTarget()
    {
        bool searchNewTarget = true;
        if (target)
        {
            float distanceToCurrentTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToCurrentTarget <= maxRange && distanceToCurrentTarget >= minRange)
            {
                searchNewTarget = false;
            }
        }

        if (searchNewTarget)
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
        }

        shootComponent.SetTarget(target);
        targetLookAtComponent.LookAt(target);
    }

    void OnMouseOver()
    {
        if (!isPlacing)
        {
            rangeCilinder.SetActive(true);
            minRangeCilinder.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        if (!isPlacing)
        {
            rangeCilinder.SetActive(false);
            minRangeCilinder.SetActive(false);
        }
    }

    public void OnStartPlacing()
    {
        isPlacing = true;
        rangeCilinder.SetActive(false);
        minRangeCilinder.SetActive(false);
        shootComponent.enabled = false;
        targetLookAtComponent.enabled = false;
    }

    public void OnEndPlacing()
    {
        isPlacing = false;
        shootComponent.enabled = true;
        targetLookAtComponent.enabled = true;
    }
}
