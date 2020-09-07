using UnityEngine;

using DG.Tweening;

public class SwapAction : Action
{
    private const float SWAP_DURATION = 0.2f;

    private Vector2Int m_tile1Position;
    private Vector2Int m_tile2Position;
    private bool m_tryMatch;

    public SwapAction(Vector2Int tile1Position, Vector2Int tile2Position, bool tryMatch)
    {
        m_tile1Position = tile1Position;
        m_tile2Position = tile2Position;
        m_tryMatch = tryMatch;
    }

    public override void OnStart()
    {
        GameObject[,] grid = GridManager.Instance.GetGrid();

        GameObject boardElement1 = grid[m_tile1Position.x, m_tile1Position.y];
        Tile tile1 = boardElement1.GetComponent<Tile>();

        GameObject boardElement2 = grid[m_tile2Position.x, m_tile2Position.y];
        Tile tile2 = boardElement2.GetComponent<Tile>();

        //swap logic: in the m_grid
        var tempGameObject = grid[m_tile1Position.x, m_tile1Position.y];
        grid[m_tile1Position.x, m_tile1Position.y] = grid[m_tile2Position.x, m_tile2Position.y];
        grid[m_tile2Position.x, m_tile2Position.y] = tempGameObject;

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
            if (m_tryMatch)
            {
                OnCompleteSwap(boardElement1, boardElement2, m_tile1Position, m_tile2Position);
            }
            else
            {
                Complete();
            }
        });
    }

    void OnCompleteSwap(GameObject boardElement1, GameObject boardElement2, Vector2Int tile1Position, Vector2Int tile2Position)
    {
        GridManager.Instance.ActionSequencer.Add(new CalculateMatchesAction(m_tile1Position, m_tile2Position));

        Complete();
    }
}
