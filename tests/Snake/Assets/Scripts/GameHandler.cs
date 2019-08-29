using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    [System.NonSerialized] public  LevelGrid levelGrid;
    public Snake snake;
    public Text textScore;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        levelGrid = new LevelGrid(20, 20);
    }
}
