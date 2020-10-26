using UnityEngine;

public class DestructibleComponent : MonoBehaviour
{
    [SerializeField]
    private int life = 100;

    private IDestructibleListener destructibleListener;

    public void Start()
    {
        destructibleListener = GetComponent<IDestructibleListener>();
    }

    public void Hit(int damage)
    {
        life -= damage;
        if (destructibleListener!=null)
        {
            destructibleListener.OnHit(life);
        }
    }

    public int GetLife()
    {
        return life;
    }
}
