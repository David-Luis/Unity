using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int health;

    private IDestructibleListener destructibleListener;

    public void Start()
    {
        health = maxHealth;
        destructibleListener = GetComponent<IDestructibleListener>();
    }

    public void Hit(int damage)
    {
        health -= damage;
        if (destructibleListener!=null)
        {
            destructibleListener.OnHit(health);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }
}
