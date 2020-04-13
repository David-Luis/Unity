using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : BaseComponent
{
    private GameObject m_player;
    private MovableComponent m_movableComponent;

    public void Awake()
    {
        m_movableComponent = GetComponent<MovableComponent>();
    }

    public void SetPlayer(GameObject player)
    {
        m_player = player;
    }

    public override void ProccessTurn()
    {
        Vector2 requestedPosition = Vector2.zero;
        if (m_player.transform.position.x < transform.position.x)
        {
            requestedPosition = new Vector2(transform.position.x - 1, transform.position.y);
        }
        else if (m_player.transform.position.x > transform.position.x)
        {
            requestedPosition = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else if (m_player.transform.position.y < transform.position.y)
        {
            requestedPosition = new Vector2(transform.position.x, transform.position.y - 1);
        }
        else if (m_player.transform.position.y > transform.position.y)
        {
            requestedPosition = new Vector2(transform.position.x, transform.position.y + 1);
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        m_movableComponent.TryMoveToCoords(currentPosition, requestedPosition);
    }
}
