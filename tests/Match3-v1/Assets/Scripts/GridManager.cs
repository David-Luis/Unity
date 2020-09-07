using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System;

//TODO: meter un booster
//TODO: separar esta clase en diferentes clases
//TODO: try to remove all the GetComponent as much as possible
//TODO: temas de resoluciones
//TODO: Add more types of matches
public class GridManager : MonoBehaviour
{
    public ActionSequencer ActionSequencer = new ActionSequencer();

    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject GamePiecePrefab;
    public int GridDimension = 8;

    public float Distance = 1.0f;

    public GameObject[,] m_grid;

    public static GridManager Instance { get; private set; }

    public HashSet<GameObject> MatchedBoardObjects { get; private set; }

    public bool CanInteract()
    {
        return !ActionSequencer.HasActions();
    }
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    void Update()
    {
        ActionSequencer.Update();
    }

    void InitGrid()
    {
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                CreateGamePiece(column, row);
            }
        }
    }

    public GameObject[,] GetGrid()
    {
        return m_grid;
    }

    public GameObject CreateGamePiece(int column, int row)
    {
        var possibleSprites = new List<Sprite>(Sprites);

        Sprite left1 = GetBoardElementAt(column - 1, row)?.GetComponent<SpriteRenderer>().sprite;
        Sprite left2 = GetBoardElementAt(column - 2, row)?.GetComponent<SpriteRenderer>().sprite;
        if (left2 != null && left1 == left2)
        {
            possibleSprites.Remove(left1);
        }

        Sprite down1 = GetBoardElementAt(column, row - 1)?.GetComponent<SpriteRenderer>().sprite;
        Sprite down2 = GetBoardElementAt(column, row - 2)?.GetComponent<SpriteRenderer>().sprite;
        if (down2 != null && down1 == down2)
        {
            possibleSprites.Remove(down1);
        }

        var sprite = possibleSprites[UnityEngine.Random.Range(0, possibleSprites.Count)];
        var gamePiece = CreateGameObject(column, row, sprite);

        m_grid[column, row] = gamePiece;

        return gamePiece;
    }

    private GameObject CreateGameObject(int column, int row, Sprite sprite)
    {
        GameObject newTile = Instantiate(GamePiecePrefab);
        var renderer = newTile.GetComponent<SpriteRenderer>();
        var tile = newTile.GetComponent<Tile>();
        newTile.name = "Tile_" + column + "_" + row;

        renderer.sprite = sprite;
        tile.Position = new Vector2Int(column, row);

        newTile.transform.parent = transform;

        newTile.transform.position = GetPositionForGameObjectAt(row, column);
        return newTile;
    }

    public Vector3 GetPositionForGameObjectAt(int row, int column)
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);

        return new Vector3(column * Distance, row * Distance, 0) + positionOffset;
    }

    public GameObject GetBoardElementAt(int column, int row)
    {
        if (column < 0 || column >= GridDimension
            || row < 0 || row >= GridDimension)
        {
            return null;
        }

       return m_grid[column, row];
    }

    public void SwapTilesAndTryMatch(Vector2Int tile1Position, Vector2Int tile2Position)
    {
        SwapTiles(tile1Position, tile2Position, tryMatch:true);
    }

    private void SwapTiles(Vector2Int tile1Position, Vector2Int tile2Position, bool tryMatch)
    {
        ActionSequencer.Add(new SwapAction(tile1Position, tile2Position, tryMatch));
    }

    public bool CalculateMatches()
    {
        MatchedBoardObjects = new HashSet<GameObject>();
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                GameObject current = GetBoardElementAt(column, row);

                List<GameObject> horizontalMatches = FindColumnMatchForTile(column, row, current);
                if (horizontalMatches.Count >= 2)
                {
                    MatchedBoardObjects.UnionWith(horizontalMatches);
                    MatchedBoardObjects.Add(current);
                }

                List<GameObject> verticalMatches = FindRowMatchForTile(column, row, current);
                if (verticalMatches.Count >= 2)
                {
                    MatchedBoardObjects.UnionWith(verticalMatches);
                    MatchedBoardObjects.Add(current);
                }
            }
        }

        foreach (var boardObject in MatchedBoardObjects)
        {
            boardObject.GetComponent<Tile>().Removed = true;
        }

        return MatchedBoardObjects.Count > 0;
    }
    
    List<GameObject> FindColumnMatchForTile(int col, int row, GameObject boardObject)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            Matchable nextColumn = GetBoardElementAt(i, row).GetComponent<Matchable>();
            if (!nextColumn.MatchWith(boardObject))
            {
                break;
            }
            result.Add(GetBoardElementAt(i, row));
        }
        return result;
    }

    List<GameObject> FindRowMatchForTile(int col, int row, GameObject boardObject)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = row + 1; i < GridDimension; i++)
        {
            Matchable nextRow = GetBoardElementAt(col, i).GetComponent<Matchable>();
            if (!nextRow.MatchWith(boardObject))
            {
                break;
            }
            result.Add(GetBoardElementAt(col, i));
        }
        return result;
    }
}
