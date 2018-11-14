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
}
