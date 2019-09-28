

using System;
using UnityEngine;


public class DownWellPlataformerController : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_MaxFallSpeed = 10f;                    // The fastest the player can travel when falling.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [SerializeField] private float m_ReleaseJumpSpeed = 3f;                  // Amount of force added when the player jumps.
    [SerializeField] private float m_KillEnemyVerticalForce = 200f;                  // Amount of force added when the player jumps.
    [SerializeField] private LayerMask m_WhatIsGround = 0;                  // A mask determining what is ground to the character
    [SerializeField] private WeaponShooter m_WeaponShooter = null;

    private const float MAX_TIME_JUMP_DELTA = 0.1f;
    private float m_currentJumpDelta = 0;


    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .3f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.

    const float k_killEnemyRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private LayerMask m_whatIsEnemy = 0;
    private bool m_killedEnemy = false;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        CheckIfKilledEnemy();

        if (!m_killedEnemy)
        {
            CheckIfGrounded();
        }

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }

    private void CheckIfKilledEnemy()
    {
        m_killedEnemy = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_killEnemyRadius, m_whatIsEnemy);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_killedEnemy = true;
                Destroy(colliders[i].gameObject);
                break;
            }
        }
    }

    private void CheckIfGrounded()
    {
        bool isInGroundNow = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isInGroundNow = m_Grounded = true;
                m_currentJumpDelta = MAX_TIME_JUMP_DELTA;
                break;
            }
        }

        if (m_Grounded && !isInGroundNow)
        {
            m_currentJumpDelta -= Time.deltaTime;
            if (m_currentJumpDelta <= 0)
            {
                m_Grounded = false;
            }
        }
        m_Anim.SetBool("Ground", m_Grounded);
    }

    public void Move(float move, bool jump, bool jumpPressed)
    {
        ControllHorizontalMove(move);

        // If the player should jump...
        if (m_killedEnemy)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_KillEnemyVerticalForce));
        }
        else if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
        else if (!m_Grounded && !jumpPressed && m_Rigidbody2D.velocity.y > m_ReleaseJumpSpeed)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_ReleaseJumpSpeed);
        }
        else if (!m_Grounded && jump)
        {
            bool canShoot = m_WeaponShooter.ShootWeapon(m_GroundCheck.position);

            if (canShoot)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_ReleaseJumpSpeed);
            }
        }
    }

    private void ControllHorizontalMove(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("Speed", Mathf.Abs(move));

        // Move the character
        m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y < -m_MaxFallSpeed ? -m_MaxFallSpeed : m_Rigidbody2D.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

