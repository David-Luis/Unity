using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        UpdateMovement();
        CheckScreenLimits();
    }

    private void UpdateMovement()
    {
        bool isPressingLeft = Input.GetAxis("Horizontal") < 0 || (Input.GetMouseButton(0) && (Input.mousePosition.x / Screen.width) <= 0.5f);
        bool isPressingRight = Input.GetAxis("Horizontal") > 0 || (Input.GetMouseButton(0) && (Input.mousePosition.x / Screen.width) > 0.5f);

        if (isPressingLeft)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
        }
        else if (isPressingRight)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void CheckScreenLimits()
    {
        float spriteWidth = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float screenLimit = Camera.main.orthographicSize * Camera.main.aspect - spriteWidth * 0.5f;

        if (transform.position.x > screenLimit)
        {
            transform.position = new Vector3(screenLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -screenLimit)
        {
            transform.position = new Vector3(-screenLimit, transform.position.y, transform.position.z);
        }
    }
}
