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

    [SerializeField]
    private float distanceToAttack = 1f;

    [SerializeField]
    private GameObject deadRewardEffect = null;

    private float attackCountDown = 0f;

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
            GameObject particleEffect = Instantiate(deadRewardEffect, transform.position, transform.rotation);
            ParticleSystem particles = particleEffect.GetComponent<ParticleSystem>();
#pragma warning disable CS0618 // Type or member is obsolete
            Destroy(particleEffect, particles.startLifetime + particles.duration);
#pragma warning restore CS0618 // Type or member is obsolete

            Systems.gameController.OnEnemyDead(coinsReward);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (destination != null && attackCountDown <= 0 && Vector3.Distance(destination.position, transform.position) <= distanceToAttack )
        {
            destination.GetComponent<DestructibleComponent>().Hit(damage);
            attackCountDown = 1f / attacksPerSecond;
        }

        attackCountDown -= Time.deltaTime;
    }

    public void AddExtraHealth(int value)
    {
        GetComponent<DestructibleComponent>().IncreaseMaxHealth(value);
    }
}
