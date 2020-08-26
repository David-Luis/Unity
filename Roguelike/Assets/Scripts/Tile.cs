using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Tile (Vector2 coords)
    {
        m_coords = coords;
        m_gameObjects = new List<GameObject>();
    }

    public Vector2 m_coords;
    public List<GameObject> m_gameObjects;
}
