using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float jumpSpeed = 7.0f;
    public float movementSpeed = 3;
    public int playerIndex = 1;

    private Rigidbody2D body;

	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        float horizontalSpeed = Input.GetAxisRaw("Horizontal" + playerIndex.ToString());

        float verticalSpeed = body.velocity.y;
        if (Input.GetButton("Jump" + playerIndex.ToString()) && CanJump())
        {
            verticalSpeed = jumpSpeed;
        }

        body.velocity = new Vector2(horizontalSpeed * movementSpeed, verticalSpeed);
    }


    private bool CanJump()
    {
        return body.velocity.y == 0;
    }
}
