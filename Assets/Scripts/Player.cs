using System;
using System.Collections;
using Abilities;
using Mobs;
using Skills;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealthPoint = 100;
    [SerializeField]
    private float m_CurrentHealthPoint = 100;
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
    private float m_DefaultGravityScale = 5f;
    [SerializeField]
    private float m_WalkingSpeed = 4f;
    [SerializeField]
    private bool m_Grounded = true;
    [SerializeField]
    private float m_JumpHeight = 3f;
    [SerializeField]
    private DoubleJump m_DoubleJump;
    [SerializeField]
    private Glide m_Glide;
    [SerializeField]
    private Dash m_Dash;
    [SerializeField]
    private EnergyExplosion m_EnergyExplosion;
    [SerializeField]
    private GameObject m_ImaginaryFriend;
    [SerializeField]
    private float m_bulletSpeed = 8f;
    [SerializeField]
    private GameObject m_bullet;
    [SerializeField]
    private LayerMask m_MobLayerMask;
    [SerializeField]
    private LayerMask m_groundLayerMask;

    [SerializeField] private PlayerAnimation m_PlayerAnimation;

    private float m_lastMovingDirection = 1f;
    private float m_LastArrowKeyPressTime;
    private RaycastHit2D  m_raycastHit;
    private Rigidbody2D m_rigidBody;
    private CapsuleCollider2D m_capsuleCollider;
    private Collider2D[] m_mobsInExplosionRadius;
    private bool m_isFacingRight = true;
    private BoxCollider2D m_feetBoxCollider2D;
    



    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_capsuleCollider = GetComponent<CapsuleCollider2D>();
        m_feetBoxCollider2D = GetComponent<BoxCollider2D>();
        m_ImaginaryFriend = GameObject.FindGameObjectWithTag("ImaginaryFriend");
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        m_lastMovingDirection = horizontalInput == 0 ? m_lastMovingDirection : horizontalInput > 0 ? 1 : -1;
        movement(horizontalInput);
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftAlt)) && GetIsGrounded() && m_Dash.GetAbilityStats().GetIsUnlocked())
        {
            StartCoroutine(dash(horizontalInput));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        if (Input.GetKey(KeyCode.Space) && m_Glide.GetAbilityStats().GetIsUnlocked())
        {
            glide();
            m_PlayerAnimation.EndGlideAnimation();
        }
        else
        {
            m_rigidBody.gravityScale = m_DefaultGravityScale;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack();
        }
        if (Input.GetKeyUp(KeyCode.LeftCommand) || Input.GetKeyUp(KeyCode.LeftAlt))
        {
            explosion();
        }
        Vector3 rayStartPosition =
            new Vector3(transform.position.x + 0.5f * m_lastMovingDirection, transform.position.y, 0);
        m_raycastHit = Physics2D.Raycast(rayStartPosition, Vector2.down, m_capsuleCollider.size.y * 1.25f,m_groundLayerMask);
        m_Grounded = m_raycastHit.collider != null;
        if (m_rigidBody.velocity.y <= 0)
        {
            m_feetBoxCollider2D.enabled = true;
        }
        checkForUnlockedSAvailabilities();
        m_PlayerAnimation.PlayPlayerAnimation(m_rigidBody.velocity.x, m_rigidBody.velocity.y, GetIsGrounded());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (m_rigidBody.velocity.y > 0)
            {
                m_feetBoxCollider2D.enabled = false;
            }
            else
            {
                m_feetBoxCollider2D.enabled = true; 
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            m_feetBoxCollider2D.enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (m_rigidBody.velocity.y > 0)
            {
                m_feetBoxCollider2D.enabled = false;
            }
            else
            {
                m_feetBoxCollider2D.enabled = true; 
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            m_feetBoxCollider2D.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (m_rigidBody.velocity.y > 0)
        {
            m_feetBoxCollider2D.enabled = false;
        }
    }

    private void movement(float i_horizontalInput)
    {
        m_rigidBody.velocity = new Vector2(i_horizontalInput * m_WalkingSpeed, m_rigidBody.velocity.y);
        if (i_horizontalInput < 0 && m_isFacingRight)
        {
            flip();
        }
        else if(i_horizontalInput > 0 && !m_isFacingRight)
        {
            flip();
        }
    }

    private void flip()
    {
        m_isFacingRight = !m_isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
   
    private void jump()
    {
        if (GetIsGrounded())
        {
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_rigidBody.gravityScale));
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, jumpForce);
            m_PlayerAnimation.JumpAnimation();
            m_DoubleJump.GetAbilityStats().SetIsAvailable(true);
        }
        else if (m_DoubleJump.GetAbilityStats().GetIsAvailable() && m_DoubleJump.GetAbilityStats().GetIsUnlocked())
        {
            if (m_rigidBody.gravityScale != m_DefaultGravityScale)
            {
                m_rigidBody.gravityScale = m_DefaultGravityScale;
            }
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_rigidBody.gravityScale));
            m_rigidBody.velocity = Vector2.up * jumpForce;
            m_PlayerAnimation.JumpAnimation();
            m_DoubleJump.GetAbilityStats().SetIsAvailable(false);
            m_DoubleJump.RunAbility(m_JumpHeight, m_rigidBody);
        }
    }

    private void glide()
    {
        if (m_Glide.GetAbilityStats().GetIsAvailable() && !GetIsGrounded() && m_rigidBody.velocity.y < 0)
        {
            m_rigidBody.gravityScale = m_Glide.GetGlideFactor();
            m_PlayerAnimation.GlideAnimation();
        }
    }

    private IEnumerator dash(float i_movingDirection)
    {
        if (m_Dash.GetAbilityStats().GetIsAvailable() && GetIsGrounded())
        {
            m_Dash.GetAbilityStats().SetIsAvailable(false);
            Vector2 dashDirection = new Vector2(transform.localScale.x * i_movingDirection, 0);
            m_rigidBody.velocity = dashDirection.normalized * m_Dash.GetDashSpeed();
            m_PlayerAnimation.DashAnimation();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(abilityCooldown(m_Dash.GetAbilityStats(),m_Dash.GetAbilityStats().GetCooldownTime()));
        }
    }

    private void attack()
    {
        GameObject bullet = Instantiate(m_bullet, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * (m_lastMovingDirection * m_bulletSpeed);
    }

    private bool GetIsGrounded()
    {
        return m_Grounded;
    }
    
    private void explosion()
    {
        float explosionRadius = m_EnergyExplosion.GetExplosionRadius();
        float explosionForce = m_EnergyExplosion.GetExplosionForce();
        Vector3 imaginaryFriendPosition = m_ImaginaryFriend.transform.position;
        
        m_mobsInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius,m_MobLayerMask);
        
        foreach (Collider2D mob in m_mobsInExplosionRadius) {
            Rigidbody2D mobRigidbody2D = mob.GetComponent<Rigidbody2D>();
            Vector2 mobDirection = (mob.transform.position - imaginaryFriendPosition).normalized;
            float mobDistance = Vector2.Distance(mob.transform.position, imaginaryFriendPosition);
            float distanceRatio = Mathf.Clamp(1 - (mobDistance / explosionRadius), 0.02f, 1);
            float calculatedExplosionForce = explosionForce * distanceRatio * transform.localScale.y;
            Debug.Log(mobDirection+"  "+calculatedExplosionForce);
            mobRigidbody2D.AddForce(mobDirection * calculatedExplosionForce,ForceMode2D.Impulse);
            mob.GetComponent<MobStats>().GetHit(m_EnergyExplosion.GetExplosionDamage());
            Debug.DrawLine(transform.position,mob.transform.position,Color.magenta,2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,m_EnergyExplosion.GetExplosionRadius());
        if (m_capsuleCollider != null && transform.position != null)
        {
            Gizmos.color = Color.red;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_lastMovingDirection, transform.position.y, 0);
            Gizmos.DrawRay(rayStartPosition,new Vector3(0,-1 * m_capsuleCollider.size.y * 1.25f,0));
        }
    }
    
    private void checkForUnlockedSAvailabilities()
    {
        if (m_Level >= m_DoubleJump.GetAbilityStats().GetAvailabilityLevel())
        {
            m_DoubleJump.GetAbilityStats().SetIsUnlocked(true);
        }
        
        if (m_Level >= m_Glide.GetAbilityStats().GetAvailabilityLevel())
        {
            m_Glide.GetAbilityStats().SetIsUnlocked(true);
        }
        
        if (m_Level >= m_Dash.GetAbilityStats().GetAvailabilityLevel())
        {
            m_Dash.GetAbilityStats().SetIsUnlocked(true);
        }
    }

    private IEnumerator abilityCooldown(AbilityStats i_Ability, float i_cooldownTime)
    {
        yield return new WaitForSeconds(i_cooldownTime);
        i_Ability.SetIsAvailable(true);
    }

    public void getHit(float i_damage)
    {
        m_CurrentHealthPoint = Mathf.Clamp(m_CurrentHealthPoint - i_damage,0,100);
    }

    public float GetMaxHealth()
    {
        return m_MaxHealthPoint;
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealthPoint;
    }
}
