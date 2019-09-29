using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBoundingController : MonoBehaviour
{
    public float m_maxSpeed = 2;
    public float m_forceMovement = 10.0f;

    private Rigidbody2D m_rigidbody2D;

    // Start is called before the first frame update
    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = GameManager.systems.player.transform.position - transform.position;
        m_rigidbody2D.AddForce(direction.normalized);

        if (m_rigidbody2D.velocity.magnitude > m_maxSpeed)
        {
            m_rigidbody2D.velocity = m_rigidbody2D.velocity.normalized * m_maxSpeed;
        }
    }
}
