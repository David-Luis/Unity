using System.Collections;
using System.Collections.Generic;
using Systems.Movable;
using UnityEngine;

public class ClickToMoveComponent : MonoBehaviour
{
    [SerializeField]
    private MovableComponent movableComponent;
    void OnMouseDown()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        movableComponent.MoveToTarget(new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0));
    }
}
