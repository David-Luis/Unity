using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform destination = null;

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

    void Update()
    {
        
    }
}
