using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableComponent : BaseComponent
{
    public void TryMoveToCoords(Vector2 fromPosition, Vector2 toPosition)
    {
        Tile currentTile = GameManager.GetDungeon().GetTileAtPosition(fromPosition);
        Tile requestedTile = GameManager.GetDungeon().GetTileAtPosition(toPosition);

        if (CanMoveToTile(requestedTile))
        {
            requestedTile.m_gameObjects.Add(gameObject);
            currentTile.m_gameObjects.Remove(gameObject);
            transform.transform.position = new Vector3(toPosition.x, toPosition.y, 0);
        }
        else
        {
            ActComponentsInGameObjectsInTile(requestedTile);
        }
    }

    private void ActComponentsInGameObjectsInTile(Tile requestedTile)
    {
        var components = gameObject.GetComponents<BaseComponent>();
        foreach (var component in components)
        {
            foreach (var gameObject in requestedTile.m_gameObjects)
            {
                var otherComponents = gameObject.GetComponents<BaseComponent>();
                foreach (var otherComponent in otherComponents)
                {
                    component.ActOnOtherComponent(otherComponent);
                }
            }
        }
    }

    bool CanMoveToTile(Tile requestedTile)
    {
        if (requestedTile.m_gameObjects.Count == 1 && requestedTile.m_gameObjects[0].CompareTag("Floor"))
        {
            return true;
        }

        foreach (var gameObject in requestedTile.m_gameObjects)
        {
            var components = gameObject.GetComponents<BaseComponent>();
            foreach (var component in components)
            {
                if (component is DestructibleComponent)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
