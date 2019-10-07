

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
    private WeaponShooter m_WeaponShooter = null;

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
    private bool m_inAirBecauseKilledOrHitEnemy = false;
    private GameObject m_enemyKilled = null;
    private BoxCollider2D m_boxCollider;

    public float m_HitByEnemyForceX = 400f;
    public float m_HitByEnemyForceY = 100f;
    private HealthController healthController;
    private bool m_hitByEnemy = false;
    private GameObject m_enemyHit = null;

    public float m_HitByEnemyThrowTime = 0.5f;
    private bool m_isThrownByEnemy = false;
    private float m_currentHitByEnemyThrowTime = 0.0f;

    public float m_invencibleTime = 1.5f;
    private float m_currentInvencibleTime = 0.0f;
    private bool m_isInvincible = false;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_boxCollider = transform.Find("BodyCollider").GetComponent<BoxCollider2D>();

        healthController = GetComponent<HealthController>();
        m_WeaponShooter = GetComponent<WeaponShooter>();
    }


    private void FixedUpdate()
    {
        if (!m_Grounded)
        {
            CheckIfKilledEnemy();
        }

        CheckIfHitByEnemy();

        if (!m_killedEnemy && !m_hitByEnemy)
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
                m_enemyKilled = gameObject;
                m_killedEnemy = true;
                m_inAirBecauseKilledOrHitEnemy = true;
                Instantiate(GameManager.systems.m_particleEnemies, colliders[i].gameObject.transform.position, Quaternion.identity, colliders[i].gameObject.transform.parent.transform);
                Destroy(colliders[i].gameObject);
                break;
            }
        }
    }

    private void CheckIfHitByEnemy()
    {
        m_hitByEnemy = false;
        m_enemyHit = null;
        if (m_isInvincible)
        {
            m_currentInvencibleTime -= Time.deltaTime;
            if (m_currentInvencibleTime<=0)
            {
                m_isInvincible = false;
            }
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapAreaAll(m_boxCollider.bounds.min, m_boxCollider.bounds.max, m_whatIsEnemy);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_enemyHit = colliders[i].gameObject;
                m_hitByEnemy = true;
                m_isInvincible = true;
                m_inAirBecauseKilledOrHitEnemy = true;
                m_isThrownByEnemy = true;
                m_currentHitByEnemyThrowTime = m_HitByEnemyThrowTime;
                m_currentInvencibleTime = m_invencibleTime;
                healthController.RemoveLife(1);

                GameManager.systems.timeManager.SlowHitByEnemy();
                GameManager.systems.shakeController.ShakeHit();
                GameManager.systems.hitOverlayController.HitByEnemy();
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
                m_inAirBecauseKilledOrHitEnemy = false;
                isInGroundNow = m_Grounded = true;
                m_currentJumpDelta = MAX_TIME_JUMP_DELTA;
                m_WeaponShooter.Reload();
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

        if (m_killedEnemy)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_KillEnemyVerticalForce));

            return;
        }

        if (m_hitByEnemy)
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            if (m_enemyHit.transform.position.x >= transform.position.x)
            {
                m_Rigidbody2D.AddForce(new Vector2(-m_HitByEnemyForceX, m_HitByEnemyForceY));
            }
            else
            {
                m_Rigidbody2D.AddForce(new Vector2(m_HitByEnemyForceX, m_HitByEnemyForceY));
            }

            return;
        }

        if (m_Grounded && jump && m_Anim.GetBool("Ground"))    // jump
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

            return;
        }

        if (!m_Grounded && !jumpPressed && !m_inAirBecauseKilledOrHitEnemy && m_Rigidbody2D.velocity.y > m_ReleaseJumpSpeed) //stop going up if player release jump button
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_ReleaseJumpSpeed);

            return;
        }

        bool shouldShoot = jump || (m_WeaponShooter.IsAutomatic() && jumpPressed);
        if (!m_Grounded && shouldShoot)   // try to shot weapon
        {
            bool canShoot = m_WeaponShooter.TryShootWeapon(m_GroundCheck.position);
            if (canShoot)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_ReleaseJumpSpeed);
            }

            return;
        }
    }

    private void ControllHorizontalMove(float move)
    {
        // The Speed animator parameter is set to the absolute value of the horizontal input.
        m_Anim.SetFloat("Speed", Mathf.Abs(move));

        // Move the character
        float speedX = move * m_MaxSpeed;
        if (m_isThrownByEnemy)
        {
            speedX = m_Rigidbody2D.velocity.x;
            m_currentHitByEnemyThrowTime -= Time.deltaTime;
            if (m_currentHitByEnemyThrowTime <= 0)
            {
                m_isThrownByEnemy = false;
                GameManager.systems.timeManager.DisableSlowHitByEnemy();
            }
        }

        m_Rigidbody2D.velocity = new Vector2(speedX, m_Rigidbody2D.velocity.y < -m_MaxFallSpeed ? -m_MaxFallSpeed : m_Rigidbody2D.velocity.y);

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

