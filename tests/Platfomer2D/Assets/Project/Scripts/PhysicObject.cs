using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour {

    public Vector2 Velocity { get; set; }

    public bool Grounded { get; private set; }

    private Rigidbody2D _rigidBody;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private Vector2 _groundNormal;

    private const float MIN_MOVE_DISTANCE = 0.001f;
    private const float SHELL_RADIUS = 0.01f;
    private const float MIN_GROUND_NORMAL = 0.65f;

    private void OnEnable()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start ()
    {
        Velocity = Vector2.zero;
        _groundNormal = Vector2.up;
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        Grounded = false;
        Vector2 movement = Velocity * Time.deltaTime;
       
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x) * movement.x;
        ApplyMovement(moveAlongGround);

        Vector2 verticalMovement = new Vector2(0, movement.y);
        ApplyMovement(verticalMovement);
    }

    private void ApplyMovement(Vector2 movement)
    {
        float distance = movement.magnitude;
        if (distance > MIN_MOVE_DISTANCE)
        {
            int count = _rigidBody.Cast(movement, _contactFilter, _hitBuffer, distance + SHELL_RADIUS);
            for (int i = 0; i < count; i++)
            {
                Vector2 currentNormal = _hitBuffer[i].normal;
                if (currentNormal.y > MIN_GROUND_NORMAL && movement.y < 0)
                {
                    Grounded = true;
                    _groundNormal = currentNormal;
                    currentNormal.x = 0;
                }

                float projection = Vector2.Dot(Velocity, currentNormal);
                if (projection < 0)
                {
                    Velocity = Velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBuffer[i].distance - SHELL_RADIUS;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidBody.position = _rigidBody.position + movement.normalized * distance;
    }
}
