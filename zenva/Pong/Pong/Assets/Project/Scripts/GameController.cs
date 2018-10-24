using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject ballPrefab;
    public Text score1Text;
    public Text score2Text;
    public float positionXForScore = 3.4f;

    private Ball ball;
    private int score1;
    private int score2;

    void Start()
    {
        SpawnBall();
    }
    
    void Update()
    {
        if (ball == null)
        {
            return;
        }

        if (ball.transform.position.x > positionXForScore)
        {
            score1++;
            SpawnBall();
        }
        else if (ball.transform.position.x < -positionXForScore)
        {
            score2++;
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        if (ball != null)
        {
            Destroy(ball.gameObject);
        }

        GameObject ballInstance = Instantiate(ballPrefab, transform);
        ball = ballInstance.GetComponent<Ball>();

        score1Text.text = score1.ToString();
        score2Text.text = score2.ToString();
    }
}
