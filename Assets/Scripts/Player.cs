using System.Collections;
using Abilities;
using Skills;
using UnityEngine;

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
    private string m_MobTag = "Mob";

    private float m_lastMovingDirection = 1f;
    private const float k_DefaultGravityScale = 1f;
    private LayerMask m_layerMask;
    private float m_LastArrowKeyPressTime;
    private RaycastHit2D  m_raycastHit;
    private Rigidbody2D m_rigidBody;
    private CapsuleCollider2D m_capsuleCollider;
    private Collider2D[] m_mobsInExplosionRadius;



    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_capsuleCollider = GetComponent<CapsuleCollider2D>();
        m_layerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        m_lastMovingDirection = horizontalInput == 0 ? m_lastMovingDirection : horizontalInput > 0 ? 1 : -1;
        calculateMovement(horizontalInput);
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
        }
        else
        {
            m_rigidBody.gravityScale = k_DefaultGravityScale;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack();
        }
        if (Input.GetKeyUp(KeyCode.LeftCommand) || Input.GetKeyUp(KeyCode.LeftAlt))
        {
            explosion();
        }
        m_raycastHit = Physics2D.Raycast(transform.position, Vector2.down, m_capsuleCollider.size.y*0.75f ,m_layerMask);
        m_Grounded = m_raycastHit.collider != null;
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
            m_rigidBody.velocity = new Vector2(m_rigidBody.velocity.x, jumpForce);
            m_DoubleJump.GetAbilityStats().SetIsAvailable(true);
        }
        else if (m_DoubleJump.GetAbilityStats().GetIsAvailable() && m_DoubleJump.GetAbilityStats().GetIsUnlocked())
        {
            if (m_rigidBody.gravityScale != k_DefaultGravityScale)
            {
                m_rigidBody.gravityScale = k_DefaultGravityScale;
            }
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_rigidBody.gravityScale));
            m_rigidBody.velocity = Vector2.up * jumpForce;
            m_DoubleJump.GetAbilityStats().SetIsAvailable(false);
            m_DoubleJump.RunAbility(m_JumpHeight, m_rigidBody);
        }
    }

    private void glide()
    {
        if (m_Glide.GetAbilityStats().GetIsAvailable() && !GetIsGrounded() && m_rigidBody.velocity.y < 0)
        {
            m_rigidBody.gravityScale = m_Glide.GetGlideFactor();
        }
    }

    private IEnumerator dash(float i_movingDirection)
    {
        if (m_Dash.GetAbilityStats().GetIsAvailable() && GetIsGrounded())
        {
            m_Dash.GetAbilityStats().SetIsAvailable(false);
            Vector2 dashDirection = new Vector2(transform.localScale.x * i_movingDirection, 0);
            m_rigidBody.velocity = dashDirection.normalized * m_Dash.GetDashSpeed();
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

    private void setIsGrounded(bool i_isGrounded)
    {
        m_Grounded = i_isGrounded;
    }
    
    private void explosion()
    {
        float explosionRadius = m_EnergyExplosion.GetExplosionRadius();
        float explosionForce = m_EnergyExplosion.GetExplosionForce();
        Vector3 imaginaryFriendPosition = m_ImaginaryFriend.transform.position;
        
        m_mobsInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        foreach (Collider2D mob in m_mobsInExplosionRadius) {
            if (mob.CompareTag(m_MobTag))
            {
                Rigidbody2D mobRigidbody2D = mob.GetComponent<Rigidbody2D>();
                Vector2 mobDirection = (mob.transform.position - imaginaryFriendPosition).normalized;
                float mobDistance = Vector2.Distance(mob.transform.position, imaginaryFriendPosition);
                float distanceRatio = Mathf.Clamp(1 - (mobDistance / explosionRadius), 0.02f, 1);
                float calculatedExplosionForce = explosionForce * distanceRatio;
                mobRigidbody2D.AddForce(mobDirection * calculatedExplosionForce,ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(m_ImaginaryFriend.transform.position,m_EnergyExplosion.GetExplosionRadius());
        if (m_capsuleCollider != null && transform.position != null)
        {
            Gizmos.DrawRay(transform.position,new Vector3(0,-1 * m_capsuleCollider.size.y *0.75f ,0));
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if ((collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform")) && 
    //         collision.transform.position.y < transform.position.y)
    //     {
    //         setIsGrounded(false);
    //         m_Glide.GetAbilityStats().SetIsAvailable(true);
    //     }
    // }
    
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
    
}
