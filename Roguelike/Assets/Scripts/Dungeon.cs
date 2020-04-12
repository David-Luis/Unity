using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

using Random = UnityEngine.Random;

public class Dungeon : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] floors;

    List<List<Tile>> m_tiles;

    public void LoadFromCSV(string path)
    {
        m_tiles = new List<List<Tile>>();

        using (var reader = new StreamReader(path))
        {
            int currentRow = 0;
            int currentCol = 0;
            while (!reader.EndOfStream)
            {
                currentCol = 0;
                var line = reader.ReadLine();
                var values = line.Split('\t');
                var rowOfTiles = new List<Tile>();
                foreach (var value in values)
                {
                    Vector3 position = new Vector3(currentCol, -currentRow, 0);
                    GameObject newGameObject;

                    if (value == "")
                    {
                        newGameObject = Instantiate(walls[Random.Range(0, walls.Length)], position, Quaternion.identity);
                    }
                    else
                    {
                        newGameObject = Instantiate(floors[Random.Range(0, floors.Length)], position, Quaternion.identity);
                    }

                    Tile tile = new Tile(new Vector2(currentCol, currentRow));
                    tile.m_gameObjects.Add(newGameObject);

                    newGameObject.transform.parent = transform;
                    rowOfTiles.Add(tile);
                    currentCol++;
                }
                m_tiles.Add(rowOfTiles);
                currentRow++;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        return m_tiles[(int)-position.y][(int)position.x];
    }
}
