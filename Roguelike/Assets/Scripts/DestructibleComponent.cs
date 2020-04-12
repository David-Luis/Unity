using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleComponent : BaseComponent
{
    public int health = 100;
    public void ReceiveAttack(int damage)
    {
        health -= damage;
        if (health<=0)
        {
            GameManager.MarkAsDestroy(gameObject);
        }
    }
}
