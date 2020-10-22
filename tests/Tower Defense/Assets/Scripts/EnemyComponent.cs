using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform destination = null;

    private Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (destination != null)
        {
            navMeshAgent.SetDestination(destination.position);
        }

        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("MoveSpeed", 1.0f);
    }

    public void SetDestination(Transform destination)
    {
        this.destination = destination;
    }

    void Update()
    {
        
    }
}
