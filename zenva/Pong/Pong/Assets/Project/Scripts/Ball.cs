using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Vector2 minSpeed = new Vector2(0.8f, 0.8f);
    public Vector2 maxSpeed = new Vector2(1.2f, 1.2f);
    public float speedMultiplier = 2;
    public float speedIncrease = 1.1f;

    private Rigidbody2D rigidBody;
    private AudioSource bounceSound;
    
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = new Vector2(Random.Range(minSpeed.x, maxSpeed.x) * (Random.value > 0.5f ? -1 : 1), Random.Range(minSpeed.y, maxSpeed.y) * (Random.value > 0.5f ? -1 : 1));
        rigidBody.velocity *= speedMultiplier;

        bounceSound = GetComponent<AudioSource>();
    }
	
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Limit")
        {
            if (collision.transform.position.y > transform.position.y && rigidBody.velocity.y > 0 || collision.transform.position.y < transform.position.y && rigidBody.velocity.y < 0)
            {
                bounceSound.Play();
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
            }
        }
        else if (collision.tag == "Paddle")
        {
            if (collision.transform.position.x < transform.position.y && rigidBody.velocity.x < 0 || collision.transform.position.x > transform.position.y && rigidBody.velocity.x > 0)
            {
                bounceSound.Play();
                rigidBody.velocity = new Vector2(-rigidBody.velocity.x * speedIncrease, rigidBody.velocity.y * speedIncrease);
            }
        }
    }
}
