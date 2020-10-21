using UnityEngine;
using UnityEngine.AI;

public class EnemyComponent : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(targetPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
