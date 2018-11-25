using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    public float GravityModifier = 1f;

    private PhysicObject _physicObject;

    private void OnEnable()
    {
        _physicObject = GetComponent<PhysicObject>();
    }

    private void FixedUpdate ()
    {
        Vector2 velocity = _physicObject.Velocity;
        velocity += GravityModifier * Physics2D.gravity;
        _physicObject.Velocity += velocity * Time.deltaTime;
    }
}
