using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid
{
    private Vector2Int foodGridPosition;
    public int width;
    public int height;
    private GameObject foodGameObject;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (CanSpawnFoodInPosition(foodGridPosition));

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
    }

    public void TryEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            CodeMonkey.CMDebug.TextPopup("aw mama", Vector3.zero);
            Object.Destroy(foodGameObject);
            SpawnFood();
            GameHandler.instance.snake.EatFood();
        }
    }

    private bool CanSpawnFoodInPosition(Vector2Int position)
    {
        return GameHandler.instance.snake.GetTilesPositions().Contains(position);
    }
}
