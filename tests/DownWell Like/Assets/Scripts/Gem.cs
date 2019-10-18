using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    public float m_maxSpeedY = 6;
    public float m_maxSpeedX = 3;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        if (Random.Range(0f,1f) < 0.5f)
        {
            m_rigidbody2D.velocity = new Vector2(m_maxSpeedX, m_maxSpeedY);
        }
        else
        {
            m_rigidbody2D.velocity = new Vector2(-m_maxSpeedX, m_maxSpeedY);
        }

        LimitSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        LimitSpeed();
    }

    private void LimitSpeed()
    {
        if (m_rigidbody2D.velocity.y > m_maxSpeedY)
        {
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, m_maxSpeedY);
        }
    }
}
