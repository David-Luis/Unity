using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour, IDestructibleListener
{
    private NavMeshAgent navMeshAgent;
    private Transform destination = null;

    [SerializeField]
    private int coinsReward = 1;
    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private float attacksPerSecond = 1f;

    private float attackCountDown = 0f;

    DestructibleComponent hitTarget = null;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (destination != null)
        {
            navMeshAgent.SetDestination(destination.position);
        }
    }

    public void SetDestination(Transform destination)
    {
        this.destination = destination;
    }

    public void OnHit(int life)
    {
        if (life <= 0)
        {
            Systems.gameController.OnEnemyDead(coinsReward);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyTarget"))
        {
            Debug.Log("HOLI");
            hitTarget = other.gameObject.GetComponent<DestructibleComponent>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyTarget"))
        {
            hitTarget = null;
        }
    }

    private void Update()
    {
        if (hitTarget != null && attackCountDown <= 0)
        {
            Debug.Log("ATTACK!");
            hitTarget.Hit(damage);
            attackCountDown = 1f / attacksPerSecond;
        }

        attackCountDown -= Time.deltaTime;
    }
}
