using UnityEngine;

public class EnemySpawnerComponent : MonoBehaviour
{
    [SerializeField]
    public GameObject enemy;
    public GameObject playerBase;



    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity, transform);
            EnemyComponent enemyComponent = newEnemy.GetComponent<EnemyComponent>();
            enemyComponent.SetDestination(playerBase.transform);
        }
    }

    void Update()
    {

    }
}
