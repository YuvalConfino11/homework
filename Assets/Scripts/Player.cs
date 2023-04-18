using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using UnityEngine;
using UnityEngine.Serialization;

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
    private float m_walkingSpeed = 2.25f;
    [SerializeField]
    private bool m_grounded = true;
    [SerializeField]
    private float m_jumpHeight = 2f;
    [SerializeField]
    private float m_glideFactor = 0.1f;
    private const float m_defultGravityScale = 1f;
    
    private Rigidbody2D m_rigidBody;
    private DoubleJump m_doubleJump = new DoubleJump();
    private Glide m_glide = new Glide();

    private void Awake()
    {
        // transform.position = new Vector3(0, 0, 0);
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForUnlockedSAvailabilities();
        calculateMovement();
        // {
        //     glide(0.1f);
        // }
        // else if (m_rigidBody.gravityScale < 1)
        // {
        //     glide(1f);
        // }
    }

    private void calculateMovement()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        // if (horizontalInput != 0 || verticalInput != 0)
        // {
        //     animator.SetBool("walk", true);
        // }
        // else
        // {
        //     animator.SetBool("walk", false);
        // }
        if (Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (getIsGrounded())
            {
                float jumpForce = Mathf.Sqrt( -2 * m_jumpHeight * (Physics2D.gravity.y * m_rigidBody.gravityScale));
                m_rigidBody.velocity = Vector2.up * jumpForce;
                // m_rigidBody.AddForce((Vector3.up * jumpForce), ForceMode2D.Impulse);
            }
            else if (m_doubleJump.getIsAvailable())
            {
                if (m_rigidBody.gravityScale != m_defultGravityScale)
                {
                    m_glide.runAbility(m_defultGravityScale, m_rigidBody);  
                }
                m_doubleJump.runAbility(m_jumpHeight,m_rigidBody);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && m_glide.getIsAvailable() && !getIsGrounded() && m_rigidBody.velocity.y < 0)
        {
            m_glide.runAbility(m_glideFactor, m_rigidBody); 
        }
        else if (m_rigidBody.gravityScale != m_defultGravityScale)
        {
            m_glide.runAbility(m_defultGravityScale, m_rigidBody);  
        }
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        transform.Translate(direction * (m_walkingSpeed * Time.deltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }



    public bool getIsGrounded()
    {
        return m_grounded;
    }
    
    public void setIsGrounded(bool i_isGrounded)
    {
        m_grounded = i_isGrounded;
    }
    
    void attack()
    {
        
    }


    public DoubleJump getDoubleJump()
    {
        return m_doubleJump;
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
            m_doubleJump.setIsAvailable(true);
            m_glide.setIsAvailable(true);
        }
    }
    
    private void CheckForUnlockedSAvailabilities()
    {
        if (m_Level >= m_doubleJump.getAvailabilityLevel())
        {
            m_doubleJump.setIsUnlocked(true);
        }
        
        if (m_Level >= m_glide.getAvailabilityLevel())
        {
            m_glide.setIsUnlocked(true);
        }
    }
}
