using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float ballReboundSpeed = 2;

    private Rigidbody2D body;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Head")
        {
            float speed = collision.transform.GetComponentInParent<Rigidbody2D>().velocity.x;
            if (speed == 0)
            {
                speed = body.velocity.x;
            }
            body.velocity = new Vector2(speed, ballReboundSpeed);
        }
        else if (collision.tag == "Floor")
        {
            body.position = new Vector2(-1.8f, 0.926f);
            body.velocity = new Vector2(0, 3);
        }
        else if (collision.tag == "Bounds")
        {
            if (collision.transform.position.x < transform.position.x && body.velocity.x < 0)
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
            }
            else if (collision.transform.position.x > transform.position.x && body.velocity.x > 0)
            {
                body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
            }
        }
    }
}
