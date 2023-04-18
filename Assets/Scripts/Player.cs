using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
using UnityEngine.Serialization;
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
    
    private Rigidbody2D m_RigidBody;
    private readonly DoubleJump r_DoubleJump = new DoubleJump();
    private readonly Glide r_Glide = new Glide();
    private readonly Dash r_Dash = new Dash();


    private void Awake()
    {
        // transform.position = new Vector3(0, 0, 0);
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        calculateMovement(horizontalInput);
        if (Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            dash(horizontalInput);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            doubleJump();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            glide(m_GlideFactor);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            glide(k_DefaultGravityScale);
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

    private void doubleJump()
    {
        if (GetIsGrounded())
        {
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_RigidBody.gravityScale));
            m_RigidBody.velocity = Vector2.up * jumpForce;
            // m_RigidBody.AddForce((Vector3.up * jumpForce), ForceMode2D.Impulse);
        }
        else if (r_DoubleJump.GetIsAvailable())
        {
            if (m_RigidBody.gravityScale != k_DefaultGravityScale)
            {
                r_Glide.RunAbility(k_DefaultGravityScale, m_RigidBody);  
            }
            r_DoubleJump.RunAbility(m_JumpHeight, m_RigidBody);
        }
    }

    private void glide(float i_glideFactor)
    {
        if (r_Glide.GetIsAvailable() && !GetIsGrounded() && m_RigidBody.velocity.y < 0)
        {
            r_Glide.RunAbility(i_glideFactor, m_RigidBody); 
        }
    }

    private void dash(float i_walkingDirection)
    {
        if (GetIsGrounded() && r_Dash.GetIsAvailable())
        {
            r_Dash.RunAbility(i_walkingDirection, m_DashSpeed, m_RigidBody);
        }
    }



    public bool GetIsGrounded()
    {
        return m_Grounded;
    }
    
    public void setIsGrounded(bool i_isGrounded)
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
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform"))
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
}
