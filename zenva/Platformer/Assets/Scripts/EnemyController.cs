using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float speed = 3;
    public float range = 2;
    public float downSpeedMulttiplier = 5;

    private Vector3 initialPosition;
    private int direction = 1;

	void Start ()
    {
        initialPosition = transform.position;
    }
	
	void Update ()
    {
        float movementY = speed * Time.deltaTime * direction;
        if (direction == -1)
        {
            movementY *= downSpeedMulttiplier;
        }
        float newY = transform.position.y + movementY;

        if (Mathf.Abs(newY - initialPosition.y) > range)
        {
            direction *= -1;
        }
        else
        {
            transform.position += Vector3.up * movementY;
        }
	}
}
