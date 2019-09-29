using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range = 10;
    public float speed = 1;

    Rigidbody2D m_rigidBody;

    private float m_originalY;

    bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.velocity = new Vector2(0, -speed);
        m_originalY = m_rigidBody.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_originalY - m_rigidBody.transform.position.y >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed)
        {
            if (other.CompareTag("breakable"))
            {
                destroyed = true;
                Instantiate(GameManager.systems.m_particleBoxes, other.transform.position, Quaternion.identity, other.transform.parent.transform);

                Destroy(gameObject);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("enemy"))
            {
                destroyed = true;
                Instantiate(GameManager.systems.m_particleEnemies, other.transform.position, Quaternion.identity, other.transform.parent.transform);

                Destroy(gameObject);
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
