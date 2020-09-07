using UnityEngine;

using DG.Tweening;


public class ResolveMatchesAction : Action
{
    private const float FALL_DURATION = 0.6f;

    public ResolveMatchesAction()
    {
    }

    public override void OnStart()
    {
        ResolveMatches();
    }

    private void ResolveMatches()
    {
        for (int column = 0; column < GridManager.Instance.GridDimension; column++)
        {
            int tilesToFall = 0;
            for (int row = 0; row < GridManager.Instance.GridDimension; row++)
            {
                GameObject boardElement = GridManager.Instance.GetBoardElementAt(column, row);
                if (boardElement.GetComponent<Tile>().Removed)
                {
                    tilesToFall++;
                    Object.Destroy(boardElement);
                }
                else if (tilesToFall > 0)
                {
                    MoveTileToEmptyPosition(boardElement, new Vector2Int(column, row - tilesToFall));
                }
            }

            int currentStartYOfsset = 0;
            for (int row = GridManager.Instance.GridDimension - tilesToFall; row < GridManager.Instance.GridDimension; row++)
            {
                GameObject gamePiece = GridManager.Instance.CreateGamePiece(column, row);
                Vector3 endPosition = new Vector3(gamePiece.transform.position.x, gamePiece.transform.position.y, gamePiece.transform.position.z);
                gamePiece.transform.position = GridManager.Instance.GetPositionForGameObjectAt(GridManager.Instance.GridDimension + currentStartYOfsset, column);
                gamePiece.transform.DOMove(endPosition, FALL_DURATION);

                currentStartYOfsset++;
            }
        }

        DOTween.Sequence().AppendInterval(FALL_DURATION * 1.2f).AppendCallback(() =>
        {
            GridManager.Instance.ActionSequencer.Add(new CalculateMatchesAction());

            Complete();
        });
    }

    private void MoveTileToEmptyPosition(GameObject boardElement, Vector2Int emptyPosition)
    {
        GameObject[,] grid = GridManager.Instance.GetGrid();

        Tile tile = boardElement.GetComponent<Tile>();

        GameObject boardElementEmpty = grid[emptyPosition.x, emptyPosition.y];

        //swap logic: in the grid
        grid[emptyPosition.x, emptyPosition.y] = boardElement;

        //swap logic: in the tile component
        tile.Position.Set(emptyPosition.x, emptyPosition.y);

        //swap logic: change the name for debugging pourposes
        boardElement.name = "Tile_" + tile.Position.x + "_" + tile.Position.y;

        //swap graphics: move to his new position
        boardElement.transform.DOMove(new Vector3(boardElementEmpty.transform.position.x, boardElementEmpty.transform.position.y, boardElementEmpty.transform.position.z), FALL_DURATION);
    }
}
