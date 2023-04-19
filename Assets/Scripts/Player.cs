using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = System.Object;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_HealthPoint = 100;
    [SerializeField]
    private float m_ManaPoint;
    [SerializeField]
    private float m_ExperiencePoints;
    [SerializeField]
    private float m_Level = 1;
    [SerializeField]
    private float m_Attack;
    [SerializeField]
    private float m_Defence;
    [SerializeField]
    private float m_WalkingSpeed = 2.25f;
    [SerializeField]
    private bool m_Grounded = true;
    [SerializeField]
    private float m_JumpHeight = 2f;
    [SerializeField]
    private float m_GlideFactor = 0.1f;
    [SerializeField]
    private float m_DashSpeed = 10f;

        
    private const float k_DefaultGravityScale = 1f;
    private float m_LastArrowKeyPressTime;
    private Ray2D m_raycast;
    private Rigidbody2D m_rigidBody;
    private BoxCollider2D m_boxCollider;
    private readonly DoubleJump r_DoubleJump = new DoubleJump();
    private readonly Glide r_Glide = new Glide();
    private readonly Dash r_Dash = new Dash();


    private void Awake()
    {
        // transform.position = new Vector2(0, 0);
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        calculateMovement(horizontalInput);
        if ((Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.LeftAlt)) && r_Dash.GetIsUnlocked())
        {
            dash(horizontalInput);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump();
        }
        if (Input.GetKey(KeyCode.UpArrow) && r_Glide.GetIsUnlocked())
        {
            glide();
        }
        else
        {
            m_rigidBody.gravityScale = k_DefaultGravityScale;
        }   
        checkForUnlockedSAvailabilities();
    }

    private void calculateMovement(float i_horizontalInput)
    {
        // if (horizontalInput != 0 || verticalInput != 0)
        // {
        //     animator.SetBool("walk", true);
        // }
        // else
        // {
        //     animator.SetBool("walk", false);
        // }
        Vector3 movingDirection = new Vector3(i_horizontalInput, 0, 0);
        transform.Translate(movingDirection * (m_WalkingSpeed * Time.deltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void jump()
    {
        if (GetIsGrounded())
        {
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_rigidBody.gravityScale));
            m_rigidBody.velocity = Vector2.up * jumpForce;
            // m_rigidBody.AddForce((Vector3.up * jumpForce), ForceMode2D.Impulse);
        }
        else if (r_DoubleJump.GetIsAvailable() && r_DoubleJump.GetIsUnlocked())
        {
            if (m_rigidBody.gravityScale != k_DefaultGravityScale)
            {
                r_Glide.RunAbility(k_DefaultGravityScale, m_rigidBody);  
            }
            r_DoubleJump.RunAbility(m_JumpHeight, m_rigidBody);
        }
    }

    private void glide()
    {
        if (r_Glide.GetIsAvailable() && !GetIsGrounded() && m_rigidBody.velocity.y < 0)
        {
            r_Glide.RunAbility(m_GlideFactor, m_rigidBody); 
        }
    }

    private void dash(float i_walkingDirection)
    {
        if (GetIsGrounded() && r_Dash.GetIsAvailable())
        {
            r_Dash.RunAbility(i_walkingDirection, m_DashSpeed, m_rigidBody);
            StartCoroutine(abilityCooldown(r_Dash,r_Dash.GetCooldownTime()));
        }
    }


    private bool GetIsGrounded()
    {
        return m_Grounded;
    }

    private void setIsGrounded(bool i_isGrounded)
    {
        m_Grounded = i_isGrounded;
    }
    
    void attack()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform"))
        {
            if (Vector3.Dot(collision.contacts[0].normal, Vector3.up) > 0.5)
            {
                setIsGrounded(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform")) && 
            collision.transform.position.y < transform.position.y)
        {
            setIsGrounded(false);
            r_DoubleJump.SetIsAvailable(true);
            r_Glide.SetIsAvailable(true);
        }
    }
    
    private void checkForUnlockedSAvailabilities()
    {
        if (m_Level >= r_DoubleJump.GetAvailabilityLevel())
        {
            r_DoubleJump.SetIsUnlocked(true);
        }
        
        if (m_Level >= r_Glide.GetAvailabilityLevel())
        {
            r_Glide.SetIsUnlocked(true);
        }
        
        if (m_Level >= r_Dash.GetAvailabilityLevel())
        {
            r_Dash.SetIsUnlocked(true);
        }
    }

    private IEnumerator abilityCooldown(Ability i_Ability, float i_cooldownTime)
    {
        yield return new WaitForSeconds(i_cooldownTime);
        i_Ability.SetIsAvailable(true);
    }
}
