using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using System.Linq;

public class Snake : MonoBehaviour
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private Direction gridMoveDirection;
    private Direction nextGridMoveDirection;
    private Quaternion nextMovementRotation;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyList;

    private void Awake()
    {
        ResetAll();
    }

    private void ResetAll()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = 0.1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Up;
        nextGridMoveDirection = Direction.Up;
        nextMovementRotation = Quaternion.Euler(0, 0, 0);
        snakeBodySize = 1;
        snakeMovePositionList = new List<SnakeMovePosition>();

        if (snakeBodyList != null)
        {
            foreach (SnakeBodyPart body in snakeBodyList)
            {
                UnityEngine.Object.Destroy(body.transform.gameObject);
            }
        }
        snakeBodyList = new List<SnakeBodyPart>();

        CreateSnakeBody();
    }

    private void Update()
    {
        HandleInput();
        HandlePosition();
    }

    private void HandlePosition()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            GameHandler.instance.levelGrid.TryEatFood(gridPosition);
            HandleBody();

            gridPosition += GetGridDirectionVector();
            gridMoveDirection = nextGridMoveDirection;
            transform.localRotation = nextMovementRotation;
        }

        bool hasCollided = CheckCollisions();
        if (hasCollided)
        {
            ResetAll();
        }
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }

    private Vector2Int GetGridDirectionVector()
    {
        Vector2Int gridMoveDirectionVector;
        switch (gridMoveDirection)
        {
            default:
            case Direction.Right:
                gridMoveDirectionVector = new Vector2Int(1, 0); break;
            case Direction.Left:
                gridMoveDirectionVector = new Vector2Int(-1, 0); break;
            case Direction.Up:
                gridMoveDirectionVector = new Vector2Int(0, 1); break;
            case Direction.Down:
                gridMoveDirectionVector = new Vector2Int(0, -1); break;
        }

        return gridMoveDirectionVector;
    }

    private bool CheckCollisions()
    {
        return gridPosition.x < 0 || gridPosition.x > GameHandler.instance.levelGrid.width ||
            gridPosition.y < 0 || gridPosition.y > GameHandler.instance.levelGrid.height || HasSelfCollided();
    }

    private bool HasSelfCollided()
    {
        return snakeMovePositionList.Count != snakeMovePositionList.Distinct().Count();

    }

    public void EatFood()
    {
        snakeBodySize++;
        gridMoveTimerMax -= 0.001f;
        GameHandler.instance.textScore.text = "Score: " + (snakeBodySize - 1);
        CreateSnakeBody();
    }

    private void CreateSnakeBody()
    {
        snakeBodyList.Add(new SnakeBodyPart(snakeBodyList.Count));
    }

    private void HandleBody()
    {
        SnakeMovePosition previousSnakeMovePosition = null;
        if (snakeMovePositionList.Count > 0)
        {
            previousSnakeMovePosition = snakeMovePositionList[0];
        }

        SnakeMovePosition snakeMovePosition = new SnakeMovePosition(gridPosition, gridMoveDirection, previousSnakeMovePosition);
        snakeMovePositionList.Insert(0, snakeMovePosition);

        if (snakeMovePositionList.Count > snakeBodySize)
        {
            snakeMovePositionList.RemoveAt(snakeBodySize);
        }

        UpdateSnakeBodyParts();
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyList.Count; i++)
        {
            snakeBodyList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 0);
                nextGridMoveDirection = Direction.Up;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 90);
                nextGridMoveDirection = Direction.Left;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 180);
                nextGridMoveDirection = Direction.Down;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, -90);
                nextGridMoveDirection = Direction.Right;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
    }

    public List<Vector2Int> GetTilesPositions()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };

        foreach (SnakeMovePosition snakeMovementPosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovementPosition.gridPosition);
        }

        return gridPositionList;
    }

    private class SnakeBodyPart
    {
        public Transform transform;
        private Vector2Int gridPosition;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(SnakeMovePosition movePosition)
        {
            float positionIncreaseX = 0f;
            float positionIncreaseY = 0f;

            gridPosition = movePosition.gridPosition;
            transform.eulerAngles = GetRotationFromDirection(movePosition, ref positionIncreaseX, ref positionIncreaseY);
            transform.position = new Vector3(gridPosition.x + positionIncreaseX, gridPosition.y + positionIncreaseY);
        }

        public Vector3 GetRotationFromDirection(SnakeMovePosition snakeMovePosition, ref float positionIncreaseX, ref float positionIncreaseY)
        {
            Direction direction = snakeMovePosition.direction;
            Direction previousDirection = snakeMovePosition.GetPreviousSnakeBodyDirection();
            Vector3 euler;
            switch (direction)
            {
                default:
                case Direction.Right:
                    switch (previousDirection)
                    {
                        default:
                            euler = new Vector3(0, 0, 90); break;
                        case Direction.Down:
                            euler = new Vector3(0, 0, 45);
                            positionIncreaseX += 0.3f;
                            positionIncreaseY += 0.3f;
                            break;
                        case Direction.Up:
                            euler = new Vector3(0, 0, -45);
                            positionIncreaseX += 0.3f;
                            positionIncreaseY -= 0.3f;
                            break;
                    }
                    break;

                case Direction.Left:
                    switch (previousDirection)
                    {
                        default:
                            euler = new Vector3(0, 0, -90); break;
                        case Direction.Down:
                            euler = new Vector3(0, 0, -45);
                            positionIncreaseX -= 0.3f;
                            positionIncreaseY += 0.3f;
                            break;
                        case Direction.Up:
                            euler = new Vector3(0, 0, 45);
                            positionIncreaseX -= 0.3f;
                            positionIncreaseY -= 0.3f;
                            break;
                    }
                    break;
                case Direction.Up:
                    switch (previousDirection)
                    {
                        default:
                            euler = new Vector3(0, 0, 0); break;
                        case Direction.Left:
                            euler = new Vector3(0, 0, 45);
                            positionIncreaseX += 0.3f;
                            positionIncreaseY += 0.3f;
                            break;
                        case Direction.Right:
                            positionIncreaseX -= 0.3f;
                            positionIncreaseY += 0.3f;
                            euler = new Vector3(0, 0, -45);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (previousDirection)
                    {
                        default:
                            euler = new Vector3(0, 0, 180); break;
                        case Direction.Left:
                            euler = new Vector3(0, 0, -45);
                            positionIncreaseX += 0.3f;
                            positionIncreaseY -= 0.3f;
                            break;
                        case Direction.Right:
                            euler = new Vector3(0, 0, 45);
                            positionIncreaseX -= 0.3f;
                            positionIncreaseY -= 0.3f;
                            break;
                    }
                    break;
            }

            return euler;
        }
    }

    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        public Vector2Int gridPosition;
        public Direction direction;

        public SnakeMovePosition(Vector2Int gridPosition, Direction direction, SnakeMovePosition previousSnakeMovePosition)
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
            this.previousSnakeMovePosition = previousSnakeMovePosition;
        }

        public Direction GetPreviousSnakeBodyDirection()
        {
            return previousSnakeMovePosition != null ? previousSnakeMovePosition.direction : Direction.Right;
        }
    }
}
