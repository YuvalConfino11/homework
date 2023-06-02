using System;
using System.Collections;
using Abilities;
using Mobs;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float m_MaxHealthPoint = 100;
    [SerializeField]
    private float m_CurrentHealthPoint = 100;
    [SerializeField]
    private float m_ManaPoint;
    [SerializeField]
    private float m_MaxManaPoint = 100;
    [SerializeField]
    private ManaBarScript m_ManaBar;
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
    private Heal m_Heal;
    [SerializeField]
    private GameObject m_ImaginaryFriend;
    [SerializeField]
    private GameObject m_Bullet;
    [SerializeField]
    private LayerMask m_MobLayerMask;
    [SerializeField]
    private LayerMask m_GroundLayerMask;
    [SerializeField]
    private LayerMask m_ObjectiveLayerMask;
    [SerializeField]
    private bool m_IsAbleToShot = true;
    [SerializeField]
    private float m_ShootingRate = 0.5f;
    [SerializeField]
    private float m_ObjectiveCollectRadius = 10f;
    [SerializeField]
    private float m_GroundRaycastDistance = 10f;


    [SerializeField] private PlayerAnimation m_PlayerAnimation;

    private float m_LastMovingDirection = 1f;
    private float m_LastArrowKeyPressTime;
    private RaycastHit2D  m_RaycastHit;
    private Rigidbody2D m_RigidBody;
    private CapsuleCollider2D m_CapsuleCollider;
    private Collider2D[] m_MobsInExplosionRadius;
    private bool m_IsFacingRight = true;
    private BoxCollider2D m_FeetBoxCollider2D;
    
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_CapsuleCollider = GetComponent<CapsuleCollider2D>();
        m_FeetBoxCollider2D = GetComponent<BoxCollider2D>();

        m_ManaPoint = GetMaxMana();
        m_ManaBar.SetMaxMana(GetMaxMana());
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        m_LastMovingDirection = horizontalInput == 0 ? m_LastMovingDirection : horizontalInput > 0 ? 1 : -1;
        movement(horizontalInput);
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftAlt)) && getIsGrounded() && m_Dash.GetAbilityStats().GetIsUnlocked())
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
            m_RigidBody.gravityScale = m_DefaultGravityScale;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack();
        }
        if (Input.GetKeyUp(KeyCode.LeftCommand) || Input.GetKeyUp(KeyCode.LeftAlt))
        {
            explosion();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            heal();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            unlockedSAvailabilitiesAndSkills();
        }
        Vector3 rayStartPosition =
            new Vector3(transform.position.x + 0.5f * m_LastMovingDirection, transform.position.y, 0);
        m_RaycastHit = Physics2D.Raycast(rayStartPosition, Vector2.down, m_GroundRaycastDistance,m_GroundLayerMask);
        m_Grounded = m_RaycastHit.collider != null;
        if (m_RigidBody.velocity.y <= 0)
        {
            m_FeetBoxCollider2D.enabled = true;
        }
        m_PlayerAnimation.PlayPlayerAnimation(m_RigidBody.velocity.x, m_RigidBody.velocity.y, getIsGrounded());

        if(m_CurrentHealthPoint == 0)
        {
            SceneManager.LoadScene("DeathScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.CompareTag("Platform"))
        {
            if (m_RigidBody.velocity.y > 0)
            {
                m_FeetBoxCollider2D.enabled = false;
            }
            else
            {
                m_FeetBoxCollider2D.enabled = true; 
            }
        }

        if (i_Collision.gameObject.CompareTag("Ground"))
        {
            m_FeetBoxCollider2D.enabled = true;
        }
    }

    private void OnCollisionStay2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.CompareTag("Platform"))
        {
            if (m_RigidBody.velocity.y > 0)
            {
                m_FeetBoxCollider2D.enabled = false;
            }
            else
            {
                m_FeetBoxCollider2D.enabled = true; 
            }
        }

        if (i_Collision.gameObject.CompareTag("Ground"))
        {
            m_FeetBoxCollider2D.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D i_Collision)
    {
        if (m_RigidBody.velocity.y > 0)
        {
            m_FeetBoxCollider2D.enabled = false;
        }
        m_CapsuleCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D i_Col)
    {
        if (i_Col.gameObject.CompareTag("Ground"))
        {
            m_CapsuleCollider.isTrigger = false;
        }
    }


    private void movement(float i_HorizontalInput)
    {
        m_RigidBody.velocity = new Vector2(i_HorizontalInput * m_WalkingSpeed, m_RigidBody.velocity.y);
        if (i_HorizontalInput < 0 && m_IsFacingRight)
        {
            flip();
        }
        else if(i_HorizontalInput > 0 && !m_IsFacingRight)
        {
            flip();
        }
    }

    private void flip()
    {
        m_IsFacingRight = !m_IsFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
   
    private void jump()
    {
        if (getIsGrounded())
        {
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_RigidBody.gravityScale));
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, jumpForce);
            m_PlayerAnimation.JumpAnimation();
            m_DoubleJump.GetAbilityStats().SetIsAvailable(true);
        }
        else if (m_DoubleJump.GetAbilityStats().GetIsAvailable() && m_DoubleJump.GetAbilityStats().GetIsUnlocked())
        {
            if (m_RigidBody.gravityScale != m_DefaultGravityScale)
            {
                m_RigidBody.gravityScale = m_DefaultGravityScale;
            }
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_RigidBody.gravityScale));
            m_RigidBody.velocity = Vector2.up * jumpForce;
            m_PlayerAnimation.JumpAnimation();
            m_DoubleJump.GetAbilityStats().SetIsAvailable(false);
            m_DoubleJump.RunAbility(m_JumpHeight, m_RigidBody);
        }
    }

    private void glide()
    {
        if (m_Glide.GetAbilityStats().GetIsAvailable() && !getIsGrounded() && m_RigidBody.velocity.y < 0)
        {
            m_RigidBody.gravityScale = m_Glide.GetGlideFactor();
        }
    }

    private IEnumerator dash(float i_MovingDirection)
    {
        if (m_Dash.GetAbilityStats().GetIsAvailable() && getIsGrounded())
        {
            m_Dash.GetAbilityStats().SetIsAvailable(false);
            Vector2 dashDirection = new Vector2(transform.localScale.x * i_MovingDirection, 0);
            m_RigidBody.velocity = dashDirection.normalized * m_Dash.GetDashSpeed();
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(abilityCooldown(m_Dash.GetAbilityStats(),m_Dash.GetAbilityStats().GetCooldownTime()));
        }
    }

    private void attack()
    {
        if (m_IsAbleToShot)
        {
            m_IsAbleToShot = false;
            GameObject bullet = Instantiate(m_Bullet, transform.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * (m_LastMovingDirection * bulletScript.GetSpeed());
            StartCoroutine(shootingCooldown());
        }
        
    }

    private bool getIsGrounded()
    {
        return m_Grounded;
    }
    
    private void explosion()
    {
        if (m_EnergyExplosion.GetSkillsStats().GetIsUnlocked() && m_EnergyExplosion.GetSkillsStats().GetIsAvailable())
        {
            float explosionRadius = m_EnergyExplosion.GetExplosionRadius();
            float explosionForce = m_EnergyExplosion.GetExplosionForce();
            Vector3 imaginaryFriendPosition = m_ImaginaryFriend.transform.position;
        
            m_MobsInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius,m_MobLayerMask);
        
            foreach (Collider2D mob in m_MobsInExplosionRadius) {
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
            SetMana(-m_EnergyExplosion.getExplosionManaPoints());
        }
    }

    private void heal()
    {
        if (m_Heal.GetSkillsStats().GetIsUnlocked() && m_Heal.GetSkillsStats().GetIsAvailable())
        {
            m_CurrentHealthPoint += m_Heal.GetHealAmount();
            SetMana(-m_Heal.GetManaPointsCost());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,m_EnergyExplosion.GetExplosionRadius());
        if (m_CapsuleCollider != null && transform.position != null)
        {
            Gizmos.color = Color.red;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_LastMovingDirection, transform.position.y, 0);
            Gizmos.DrawRay(rayStartPosition,new Vector3(0,-1 * m_GroundRaycastDistance,0));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,m_ObjectiveCollectRadius);
    }
    
    private void unlockedSAvailabilitiesAndSkills()
    {
        Collider2D objectivesInRadius = Physics2D.OverlapCircle(transform.position, m_ObjectiveCollectRadius,m_ObjectiveLayerMask);
        if (objectivesInRadius != null)
        {
            switch (objectivesInRadius.name)
            {
                case "DoubleJump":
                    m_DoubleJump.GetAbilityStats().SetIsUnlocked(true);
                    m_DoubleJump.GetAbilityStats().SetIsAvailable(true);
                    break;
                case "Dash":
                    m_Dash.GetAbilityStats().SetIsUnlocked(true);
                    m_Dash.GetAbilityStats().SetIsAvailable(true);
                    break;
                case "EnergyExplosion":
                    m_EnergyExplosion.GetSkillsStats().SetIsUnlocked(true);
                    m_EnergyExplosion.GetSkillsStats().SetIsAvailable(true);
                    break;
                case "Glide":
                    m_Glide.GetAbilityStats().SetIsUnlocked(true);
                    m_Glide.GetAbilityStats().SetIsAvailable(true);
                    break;
                case "Heal":
                    m_Heal.GetSkillsStats().SetIsUnlocked(true);
                    m_Heal.GetSkillsStats().SetIsAvailable(true);
                    break;
            }
            Destroy(objectivesInRadius.gameObject);
        }
    }

    private IEnumerator abilityCooldown(AbilityStats i_Ability, float i_CooldownTime)
    {
        yield return new WaitForSeconds(i_CooldownTime);
        i_Ability.SetIsAvailable(true);
    }
    
    private IEnumerator shootingCooldown()
    {
        yield return new WaitForSeconds(m_ShootingRate);

        m_IsAbleToShot = true;
    }

    public void getHit(float i_Damage)
    {
        m_CurrentHealthPoint = Mathf.Clamp(m_CurrentHealthPoint - i_Damage,0,100);
    }

    public float GetMaxHealth()
    {
        return m_MaxHealthPoint;
    }

    public float GetCurrentHealth()
    {
        return m_CurrentHealthPoint;
    }

    public float GetMana()
    {
        return m_ManaPoint;
    }
    public void SetMana(float i_Mana)
    {
        m_ManaPoint += i_Mana;
        m_ManaPoint = Math.Clamp(m_ManaPoint, 0f, 100f);
        m_ManaBar.SetMana(m_ManaPoint);
    }
    public float GetMaxMana()
    {
        return m_MaxManaPoint;
    }

}
