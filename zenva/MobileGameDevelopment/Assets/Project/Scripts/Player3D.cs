using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{

    public float speed;


    void Update()
    {
        UpdateMovement();
        CheckScreenLimits();
    }

    private void UpdateMovement()
    {
        Vector2 relativeMousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        bool isPressingLeft = Input.GetAxis("Horizontal") < 0 || (Input.GetMouseButton(0) && relativeMousePosition.x <= 0.5f);
        bool isPressingRight = Input.GetAxis("Horizontal") > 0 || (Input.GetMouseButton(0) && relativeMousePosition.x > 0.5f);

        if (isPressingLeft)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(-speed, 0);
        }
        else if (isPressingRight)
        {
            GetComponent<Rigidbody>().velocity = new Vector2(speed, 0);
        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector2.zero;
        }
    }

    private void CheckScreenLimits()
    {
        float zDistance = Camera.main.transform.position.z - transform.position.z;

        float leftLimit = Camera.main.ScreenToWorldPoint(new Vector3(
            0,
            0,
            -zDistance / (Mathf.Cos(Camera.main.transform.localEulerAngles.x)))).x;

        float rightLimit = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, 
            0, 
            -zDistance / (Mathf.Cos(Camera.main.transform.localEulerAngles.x)))).x;

        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < leftLimit)
        {
            transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
        }
    }

};
