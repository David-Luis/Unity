using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

//TODO: create a Matchable component, that will be used for matching
//TODO: try to remove all the GetComponent as much as possible
public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject GamePiecePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;

    private GameObject[,] m_grid;

    public static GridManager Instance { get; private set; }

    private const float SWAP_DURATION = 0.2f;

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

    }

    void InitGrid()
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++)
            {
                CreateGamePiece(positionOffset, row, column);
            }
        }
    }

    private void CreateGamePiece(Vector3 positionOffset, int row, int column)
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

        var sprite = possibleSprites[Random.Range(0, possibleSprites.Count)]; ;
        var gamePiece = CreateGameObject(positionOffset, row, column, sprite);

        m_grid[column, row] = gamePiece;
    }

    private GameObject CreateGameObject(Vector3 positionOffset, int row, int column, Sprite sprite)
    {
        GameObject newTile = Instantiate(GamePiecePrefab);
        var renderer = newTile.GetComponent<SpriteRenderer>();
        var tile = newTile.GetComponent<Tile>();
        newTile.name = "Tile_" + column + "_" + row;

        renderer.sprite = sprite;
        tile.Position = new Vector2Int(column, row);

        newTile.transform.parent = transform;
        newTile.transform.position = new Vector3(column * Distance, row * Distance, 0) + positionOffset;
        return newTile;
    }

    GameObject GetBoardElementAt(int column, int row)
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
        GameObject boardElement1 = m_grid[tile1Position.x, tile1Position.y];
        Tile tile1 = boardElement1.GetComponent<Tile>();

        GameObject boardElement2 = m_grid[tile2Position.x, tile2Position.y];
        Tile tile2 = boardElement2.GetComponent<Tile>();

        //swap logic: in the grid
        var tempGameObject = m_grid[tile1Position.x, tile1Position.y];
        m_grid[tile1Position.x, tile1Position.y] = m_grid[tile2Position.x, tile2Position.y];
        m_grid[tile2Position.x, tile2Position.y] = tempGameObject;

        //swap logic: in the tile component
        var tempTile = new Vector2Int(tile1.Position.x, tile1.Position.y);
        tile1.Position.Set(tile2.Position.x, tile2.Position.y);
        tile2.Position.Set(tempTile.x, tempTile.y);

        //swap logic: change the name for debugging pourposes
        boardElement1.name = "Tile_" + tile1.Position.x + "_" + tile1.Position.y;
        boardElement2.name = "Tile_" + tile2.Position.x + "_" + tile2.Position.y;

        //swap graphics: move to his new position
        boardElement1.transform.DOMove(new Vector3(boardElement2.transform.position.x, boardElement2.transform.position.y, boardElement2.transform.position.z), SWAP_DURATION);
        boardElement2.transform.DOMove(new Vector3(boardElement1.transform.position.x, boardElement1.transform.position.y, boardElement1.transform.position.z), SWAP_DURATION).OnComplete(() =>
        {
            if (tryMatch)
            {
                OnCompleteSwap(boardElement1, boardElement2, tile1Position, tile2Position);
            }
        });
    }

    void OnCompleteSwap(GameObject boardElement1, GameObject boardElement2, Vector2Int tile1Position, Vector2Int tile2Position)
    {
        bool foundMatch = CheckMatches();
        if (foundMatch)
        {
            FillHoles();
        }
        else
        {
            SwapTiles(tile1Position, tile2Position, tryMatch:false);
        }
    }

    bool CheckMatches()
    {
        HashSet<GameObject> matchedBoardObjects = new HashSet<GameObject>(); 
        for (int row = 0; row < GridDimension; row++)
        {
            for (int column = 0; column < GridDimension; column++) 
            {
                GameObject current = GetBoardElementAt(column, row); 

                List<GameObject> horizontalMatches = FindColumnMatchForTile(column, row, current); 
                if (horizontalMatches.Count >= 2)
                {
                    matchedBoardObjects.UnionWith(horizontalMatches);
                    matchedBoardObjects.Add(current); 
                }

                List<GameObject> verticalMatches = FindRowMatchForTile(column, row, current); 
                if (verticalMatches.Count >= 2)
                {
                    matchedBoardObjects.UnionWith(verticalMatches);
                    matchedBoardObjects.Add(current);
                }
            }
        }

        
        foreach (var boardObject in matchedBoardObjects) 
        {
            //TODO: change it to Tile.removed = True and use later to fill the holes
            //also after removing the holes, remove all the Tile with the .removed
            //later do this with a pool instead of removing adding
            boardObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        return matchedBoardObjects.Count > 0; 
    }

    List<GameObject> FindColumnMatchForTile(int col, int row, GameObject boardObject)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = col + 1; i < GridDimension; i++)
        {
            SpriteRenderer nextColumn = GetBoardElementAt(i, row).GetComponent<SpriteRenderer>();
            if (nextColumn.sprite != boardObject.GetComponent<SpriteRenderer>().sprite)
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
            SpriteRenderer nextRow = GetBoardElementAt(col, i).GetComponent<SpriteRenderer>();
            if (nextRow.sprite != boardObject.GetComponent<SpriteRenderer>().sprite)
            {
                break;
            }
            result.Add(GetBoardElementAt(col, i));
        }
        return result;
    }

    void FillHoles()
    {
        //TODO: change this to not change the sprite renderer, but do the same as in the swap
        // swap logic: in the grid
        // swap logic: in the tile component
        // swap logic: change the name for debugging pourposes
        // swap graphics: move to his new position
        for (int column = 0; column < GridDimension; column++)
        {
            for (int row = 0; row < GridDimension; row++) 
            {
                while (GetBoardElementAt(column, row)?.GetComponent<SpriteRenderer>().sprite == null) 
                {
                    for (int filler = row; filler < GridDimension - 1; filler++) 
                    {
                        SpriteRenderer current = GetBoardElementAt(column, filler).GetComponent<SpriteRenderer>(); 
                        SpriteRenderer next = GetBoardElementAt(column, filler + 1).GetComponent<SpriteRenderer>();
                        current.sprite = next.sprite;
                    }
                    SpriteRenderer last = GetBoardElementAt(column, GridDimension - 1).GetComponent<SpriteRenderer>();
                    last.sprite = Sprites[Random.Range(0, Sprites.Count)]; 
                }
            }
        }
    }
}
