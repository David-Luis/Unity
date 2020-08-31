using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

//TODO: Playback secuences (pensar otro nombre), con su "complete" para pasar a la siguiente, así haremos:
//TODO: ANIMACIÓN DE DESTRUCCIÓN
//TODO: ver si puedo usar lo de playback secuences también para hacer el swap y el match
//TODO: separar lógica en diferentes clases
//TODO: try to remove all the GetComponent as much as possible
public class GridManager : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    public GameObject GamePiecePrefab;
    public int GridDimension = 8;
    public float Distance = 1.0f;

    private GameObject[,] m_grid;

    public static GridManager Instance { get; private set; }

    private const float FALL_DURATION = 0.6f;
    private const float SWAP_DURATION = 0.2f;
    
    public EState State;

    public enum EState
    {
        Ready,
        Swapping,
        Matching
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        State = EState.Ready;
        m_grid = new GameObject[GridDimension, GridDimension];
        InitGrid();
    }

    void Update()
    {
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

    private GameObject CreateGamePiece(int column, int row)
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

        var sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
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

    private Vector3 GetPositionForGameObjectAt(int row, int column)
    {
        Vector3 positionOffset = transform.position - new Vector3(GridDimension * Distance / 2.0f, GridDimension * Distance / 2.0f, 0);
        return new Vector3(column * Distance, row * Distance, 0) + positionOffset;
    }

    private GameObject GetBoardElementAt(int column, int row)
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
        State = EState.Swapping;

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
            else
            {
                State = EState.Ready;
            }
        });
    }

    private void MoveTileToEmptyPosition(GameObject boardElement, Vector2Int emptyPosition)
    {
        Tile tile = boardElement.GetComponent<Tile>();

        GameObject boardElementEmpty = m_grid[emptyPosition.x, emptyPosition.y];

        //swap logic: in the grid
        m_grid[emptyPosition.x, emptyPosition.y] = boardElement;

        //swap logic: in the tile component
        tile.Position.Set(emptyPosition.x, emptyPosition.y);

        //swap logic: change the name for debugging pourposes
        boardElement.name = "Tile_" + tile.Position.x + "_" + tile.Position.y;

        //swap graphics: move to his new position
        boardElement.transform.DOMove(new Vector3(boardElementEmpty.transform.position.x, boardElementEmpty.transform.position.y, boardElementEmpty.transform.position.z), FALL_DURATION);
    }

    void OnCompleteSwap(GameObject boardElement1, GameObject boardElement2, Vector2Int tile1Position, Vector2Int tile2Position)
    {
        bool foundMatch = ResolveMatches();
        if (!foundMatch)
        {
            SwapTiles(tile1Position, tile2Position, tryMatch: false);
        }
    }

    //TODO: Add more types of matches
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
            boardObject.GetComponent<Tile>().Removed = true;
        }

        return matchedBoardObjects.Count > 0; 
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

    bool ResolveMatches()
    {
        bool foundMatch = CheckMatches();
        if (!foundMatch)
        {
            State = EState.Ready;
            return false;
        }

        State = EState.Matching;
        for (int column = 0; column < GridDimension; column++)
        {
            int tilesToFall = 0;
            for (int row = 0; row < GridDimension; row++)
            {
                GameObject boardElement = GetBoardElementAt(column, row);
                if (boardElement.GetComponent<Tile>().Removed)
                {
                    tilesToFall++;
                    Destroy(boardElement);
                }
                else if (tilesToFall > 0)
                {
                    MoveTileToEmptyPosition(boardElement, new Vector2Int(column, row - tilesToFall));
                }
            }

            int currentStartYOfsset = 0;
            for (int row = GridDimension-tilesToFall; row < GridDimension; row++)
            {
                GameObject gamePiece = CreateGamePiece(column, row);
                Vector3 endPosition = new Vector3(gamePiece.transform.position.x, gamePiece.transform.position.y, gamePiece.transform.position.z);
                gamePiece.transform.position = GetPositionForGameObjectAt(GridDimension + currentStartYOfsset, column);
                gamePiece.transform.DOMove(endPosition, FALL_DURATION);

                currentStartYOfsset++;
            }


        }

        Invoke("ResolveMatches", FALL_DURATION*1.2f);

        return true;
    }
}
