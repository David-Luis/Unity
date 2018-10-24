using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

    public float speed = 3.0f;
    public int playerIndex = 1;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        float verticalMovement = Input.GetAxisRaw("Vertical" + playerIndex.ToString());
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, verticalMovement * speed);
    }
}
