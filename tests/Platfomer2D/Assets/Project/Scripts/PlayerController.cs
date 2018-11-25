using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private PhysicObject _physicObject;

    private void OnEnable()
    {
        _physicObject = GetComponent<PhysicObject>();
    }

    private void Update()
    {
        float move = Input.GetAxis("Horizontal");

        float walkVelocity = 0;
        walkVelocity += move * walkSpeed;

        float jumpVelocity = _physicObject.Velocity.y;
        if (Input.GetButtonDown("Jump") && _physicObject.Grounded)
        {
            jumpVelocity = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (jumpVelocity > 0)
            {
                jumpVelocity *= 0.5f;
            }
        }

        _physicObject.Velocity = new Vector2(walkVelocity, jumpVelocity);
    }
}
