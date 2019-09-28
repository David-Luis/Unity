using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMovement : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private int m_direction = 1;
    public float m_speed = 1;

    [SerializeField] private LayerMask m_whatIsGround = 0;

    private Transform m_edgeDetector;
    private float m_edgeDetectorRadius;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_edgeDetector = transform.Find("edge_detector");
        m_edgeDetectorRadius = m_edgeDetector.GetComponent<CircleCollider2D>().radius;
    }

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_edgeDetector.position, m_edgeDetectorRadius, m_whatIsGround);
        bool detectedGround = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                detectedGround = true;
                break;
            }
        }

        if (!detectedGround)
        {
            Flip();
        }

        Debug.Log(m_direction);
        m_rigidbody2D.velocity = new Vector2(m_direction * m_speed, m_rigidbody2D.velocity.y);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_direction *= -1;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
