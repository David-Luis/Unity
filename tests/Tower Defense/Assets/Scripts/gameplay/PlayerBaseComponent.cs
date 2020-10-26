using UnityEngine;

public class PlayerBaseComponent : MonoBehaviour, IDestructibleListener
{
    public void OnHit(int life)
    {
        if (life <= 0)
        {
            Systems.gameController.OnPlayerDead();
        }
    }
}
