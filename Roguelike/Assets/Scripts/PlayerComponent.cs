using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : BaseComponent
{
    private MovableComponent m_movableComponent;
    public void Awake()
    {
        m_movableComponent = GetComponent<MovableComponent>();
    }

    public void Update()
    {
        bool processTurn = false;
        Vector2 requestedPosition = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            requestedPosition = new Vector2(transform.position.x - 1, transform.position.y);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            requestedPosition = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            requestedPosition = new Vector2(transform.position.x, transform.position.y + 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            requestedPosition = new Vector2(transform.position.x, transform.position.y - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            processTurn = true;
        }

        if (requestedPosition != Vector2.zero)
        {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            m_movableComponent.TryMoveToCoords(currentPosition, requestedPosition);
            processTurn = true;
        }

        if (processTurn)
        {
            GameManager.ProcessTurn();
        }
    }
}
