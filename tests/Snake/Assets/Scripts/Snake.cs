using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using System.Linq;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int nextGridMoveDirection;
    private Quaternion nextMovementRotation;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;

    private void Awake()
    {
        ResetAll();
    }

    private void ResetAll()
    {
        gridPosition = new Vector2Int(10, 10);
        gridMoveTimerMax = 0.1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(0, 1);
        nextGridMoveDirection = new Vector2Int(0, 1);
        nextMovementRotation = Quaternion.Euler(0, 0, 0);
        snakeBodySize = 1;
        snakeMovePositionList = new List<Vector2Int>();
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

            gridPosition += gridMoveDirection;
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
        GameHandler.instance.textScore.text = "Score: " + (snakeBodySize-1);
    }

    private void HandleBody()
    {
        snakeMovePositionList.Insert(0, gridPosition);

        if (snakeMovePositionList.Count > snakeBodySize)
        {
            snakeMovePositionList.RemoveAt(snakeBodySize);
        }
        for (int i = 0; i < snakeMovePositionList.Count; i++)
        {
            Vector2Int snakeMovePosition = snakeMovePositionList[i];
            World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
            FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection.y == 0)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 0);
                nextGridMoveDirection.x = 0;
                nextGridMoveDirection.y = 1;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection.x == 0)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 90);
                nextGridMoveDirection.x = -1;
                nextGridMoveDirection.y = 0;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection.y == 0)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, 180);
                nextGridMoveDirection.x = 0;
                nextGridMoveDirection.y = -1;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection.x == 0)
            {
                nextMovementRotation = Quaternion.Euler(0, 0, -90);
                nextGridMoveDirection.x = 1;
                nextGridMoveDirection.y = 0;
                gridMoveTimer = gridMoveTimerMax;
            }
        }
    }

    public List<Vector2Int> GetTilesPositions()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }
}
