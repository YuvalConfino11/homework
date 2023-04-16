using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;
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
    private bool m_canJump = true;
    [SerializeField] 
    private bool m_canDoubleJump = false;
    [SerializeField]
    private float m_walkingSpeed = 2.25f;
    [SerializeField]
    private float k_jump = 5f;
    private Rigidbody2D m_rigidBody;
    private short m_DoubleJumpCount = 2;

    private void Awake()
    {
        // transform.position = new Vector3(0, 0, 0);
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        if (Input.GetKeyDown(KeyCode.Q) && m_Level >= DoubleJump.GetAvailableFromLevel())
        {
            StartCoroutine(doubleJump());
        }
        if (Input.GetKey(KeyCode.LeftShift) && m_Level >= Glide.GetAvailableFromLevel() && m_rigidBody.velocity.y <= 0)
        {
            glide(0.1f);
        }
        else if (m_rigidBody.gravityScale < 1)
        {
            glide(1f);
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && (m_canJump || m_DoubleJumpCount < 2))
        {
            m_rigidBody.AddForce(Vector3.up * k_jump, ForceMode2D.Impulse);
            m_canJump = false;
            m_DoubleJumpCount++;
            Debug.Log(m_DoubleJumpCount);
        }
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        transform.Translate(direction * (m_walkingSpeed * Time.deltaTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    

    private IEnumerator doubleJump()
    {
        m_canDoubleJump = true;
        m_DoubleJumpCount = 0;
        yield return new WaitForSeconds(DoubleJump.GetCooldownTime());
        m_canDoubleJump = false;
    }

    private void glide(float i_gravityScale)
    {
        m_rigidBody.gravityScale = i_gravityScale;
    }
    
    void attack()
    {
        
    }

    public void setCanJump(bool i_CanJump)
    {
        m_canJump = i_CanJump;
    }

    public bool getCanDoubleJump()
    {
        return m_canDoubleJump;
    }
    
    public void resetDoubleJumpCount()
    {
        m_DoubleJumpCount = 0;
    }
    
}
