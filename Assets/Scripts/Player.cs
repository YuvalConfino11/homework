using System;
using System.Collections;
using Abilities;
using Mobs;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


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
    private float m_CurrentWalkingSpeed = 4f;
    [SerializeField]
    private float m_RegularWalkingSpeed = 4f;
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
    [SerializeField]
    private bool[] m_PlayerGotKey = new bool[3];
    [SerializeField] 
    private PlayerAnimation m_PlayerAnimation;
    [SerializeField]
    private BoxCollider2D m_FeetBoxCollider2D;
    [SerializeField]
    private bool m_movingEnabled = true;
    [SerializeField]
    private float m_movingdisabledTime = 0.8f;
    [SerializeField]
    private Animator m_HealAnimator;
    [SerializeField]
    private Animator m_ExplosionAnimator;
    [SerializeField]
    private float m_HealAnimatorCooldown = 0.5f;
    [SerializeField]
    private float m_ExplosionAnimatorCooldown = 0.8f;
    [SerializeField]
    private Light2D m_PlayerLight;
    [SerializeField]
    private Image m_HitScreen;
    [SerializeField]
    private GameObject m_ProjectileStartingPosition;
    [SerializeField]
    private GameObject m_StoneWall;
    [SerializeField]
    private float m_DebrisTime;
    [SerializeField]
    private AudioManager m_audioManager;
    private BoxCollider2D m_BoxCollider;



    private float m_LastMovingDirection = 1f;
    private float m_LastArrowKeyPressTime;
    private RaycastHit2D  m_RaycastHit;
    private Rigidbody2D m_RigidBody;
    private Collider2D[] m_MobsInExplosionRadius;
    private bool m_IsFacingRight = true;
    private bool m_IsOnDash = false;
   
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_BoxCollider = GetComponent<BoxCollider2D>();

        m_ManaPoint = GetMaxMana();
        m_ManaBar.SetMaxMana(GetMaxMana());
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("Happy ver1");
        }
        
        m_HitScreen.gameObject.SetActive(false);
        m_CurrentWalkingSpeed = m_RegularWalkingSpeed;
        StartCoroutine(MovmentDisabled(3f));


    }
    
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        m_LastMovingDirection = horizontalInput == 0 ? m_LastMovingDirection : horizontalInput > 0 ? 1 : -1;
        if (m_movingEnabled)
        {
            movement(horizontalInput);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && getIsGrounded() && m_Dash.GetAbilityStats().GetIsUnlocked())
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
            m_RigidBody.gravityScale = m_DefaultGravityScale;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            attack();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_PlayerAnimation.SetAttackAnimation(false);
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
            interact();
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
            AudioManager.Instance.musicSource.Stop();
            SceneManager.LoadScene("DeathScene");
        }
        
        AdjustPlayerLightIntensity();
    }

    private void AdjustPlayerLightIntensity()
    {
        m_PlayerLight.intensity = (1f - (m_CurrentHealthPoint / m_MaxHealthPoint)) * 0.1f;
      
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
        
        if (i_Collision.gameObject.CompareTag("SpiderWeb"))
        {
            m_BoxCollider.isTrigger = false;
            m_CurrentWalkingSpeed = m_RegularWalkingSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D i_Col)
    {
        if (i_Col.gameObject.CompareTag("Ground"))
        {
            m_BoxCollider.isTrigger = false;
        }

        if (i_Col.gameObject.CompareTag("Key"))
        {
            Destroy(i_Col.gameObject);
        }
        
        if (i_Col.gameObject.CompareTag("ResetWall"))
        {
            ResetAbility(m_Dash.GetAbilityStats());
            ResetAbility(m_DoubleJump.GetAbilityStats());
            ResetSkill(m_EnergyExplosion.GetSkillsStats());
            ResetAbility(m_Glide.GetAbilityStats());
            ResetSkill(m_Heal.GetSkillsStats());
        }
        
        if (i_Col.gameObject.CompareTag("Spike"))
        {
            StartCoroutine(invicible());
        }
        
        if (i_Col.gameObject.CompareTag("Change Scene Wall"))
        {
            SceneManager.LoadScene("Map");
        }
        
        if (i_Col.gameObject.CompareTag("SpiderWeb"))
        {
            m_BoxCollider.isTrigger = true;
            m_CurrentWalkingSpeed = m_RegularWalkingSpeed / 2;
        }
        if (i_Col.gameObject.CompareTag("TriggerMusic"))
        {
            StartCoroutine(m_audioManager.ChangeMusic("TheChase"));
        }
        if (i_Col.gameObject.CompareTag("TriggerMusicExit"))
        {
            StartCoroutine(m_audioManager.ChangeMusic("Happy ver1"));
        }
    }

    private void OnTriggerExit2D(Collider2D i_Col)
    {
        if (i_Col.gameObject.CompareTag("SpiderWeb"))
        {
            m_BoxCollider.isTrigger = false;
            m_CurrentWalkingSpeed = m_RegularWalkingSpeed;
        }
    }


    private void movement(float i_HorizontalInput)
    {
        m_RigidBody.velocity = new Vector2(i_HorizontalInput * m_CurrentWalkingSpeed, m_RigidBody.velocity.y);
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
        m_ImaginaryFriend.transform.Rotate(0f, 180f, 0f);
    }
   
    private void jump()
    {
        if (getIsGrounded())
        {
            float jumpForce = Mathf.Sqrt( -2 * m_JumpHeight * (Physics2D.gravity.y * m_RigidBody.gravityScale));
            m_RigidBody.velocity = new Vector2(m_RigidBody.velocity.x, jumpForce);
            m_PlayerAnimation.JumpAnimation();
            m_DoubleJump.GetAbilityStats().SetIsAvailable(true);
            AudioManager.Instance.PlaySFX("Jump");
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
            m_PlayerAnimation.GlideAnimation();
        }
    }

    private IEnumerator dash(float i_MovingDirection)
    {
        if (m_Dash.GetAbilityStats().GetIsAvailable() && getIsGrounded())
        {
           
            StartCoroutine(MovmentDisabled(m_movingdisabledTime));
            StartCoroutine(invicible(0.5f));
            m_Dash.GetAbilityStats().SetIsAvailable(false);
            float dashTimer = 0;
            float dashDuration = m_Dash.DashTime;
            m_PlayerAnimation.DashAnimation();
            while(dashDuration > dashTimer)
            {
                dashTimer += Time.deltaTime;
                Vector2 dashDirection = new Vector2(i_MovingDirection * m_Dash.DashDistance, 0);
                m_RigidBody.AddForce(dashDirection * m_Dash.DashDistance);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(abilityCooldown(m_Dash.GetAbilityStats(),m_Dash.GetAbilityStats().GetCooldownTime()));

        }
    }


    public IEnumerator Knockback(float i_KnockbackDuration , float i_KnockbackPower , Transform i_ObjectTransform)
    {
        float timer = 0;
        m_movingEnabled = false;
        StartCoroutine(MovmentDisabled(m_movingdisabledTime));

        while(i_KnockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 dir = new Vector2(i_ObjectTransform.transform.position.x - transform.position.x,0);
            m_RigidBody.AddForce(-dir * i_KnockbackPower);
        }
        
        yield return 0;
    }

    private void attack()
    {
        if (m_IsAbleToShot)
        {
            m_IsAbleToShot = false;
            GameObject bullet = Instantiate(m_Bullet, m_ProjectileStartingPosition.transform.position, transform.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            m_PlayerAnimation.SetAttackAnimation(true);
            bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * (m_LastMovingDirection * bulletScript.GetSpeed());
            AudioManager.Instance.PlaySFX("Shoot");
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
            AudioManager.Instance.PlaySFX("Explosion");
        
            m_MobsInExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius,m_MobLayerMask);
            foreach (Collider2D mob in m_MobsInExplosionRadius) {
                if (mob.CompareTag("StoneWall"))
                {
                   foreach (Transform child in m_StoneWall.transform)
                    {
                        child.transform.GetComponent<Rigidbody2D>().gravityScale=2;
                        child.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                        child.gameObject.layer = 21;
                        child.transform.GetComponent<SpriteRenderer>().sortingLayerName = "Mob";
                        Fadeout(11f, child.gameObject);
                    }
                    Destroy(m_StoneWall, m_DebrisTime);
                }
                else
                {
                    Rigidbody2D mobRigidbody2D = mob.GetComponent<Rigidbody2D>();
                    Vector2 mobDirection = (mob.transform.position - imaginaryFriendPosition).normalized;
                    float mobDistance = Vector2.Distance(mob.transform.position, imaginaryFriendPosition);
                    float distanceRatio = Mathf.Clamp(1 - (mobDistance / explosionRadius), 0.02f, 1);
                    float calculatedExplosionForce = explosionForce * distanceRatio * transform.localScale.y;
                    mobRigidbody2D.AddForce(mobDirection * calculatedExplosionForce,ForceMode2D.Impulse);
                    mob.GetComponent<MobStats>().GetHit(m_EnergyExplosion.GetExplosionDamage());
                    Debug.DrawLine(transform.position,mob.transform.position,Color.magenta,2);
                }
                
            }
            SetMana(-m_EnergyExplosion.getExplosionManaPoints());
            m_ExplosionAnimator.SetBool("IsExplosionPulseActive",true);
            StartCoroutine(AnimatorCooldown("IsExplosionPulseActive", m_ExplosionAnimator, m_ExplosionAnimatorCooldown));
        }
    }

    private void heal()
    {
        if (m_Heal.GetSkillsStats().GetIsUnlocked() && m_Heal.GetSkillsStats().GetIsAvailable())
        {
            m_CurrentHealthPoint += m_Heal.GetHealAmount();
            SetMana(-m_Heal.GetManaPointsCost());
            AudioManager.Instance.PlaySFX("Angel");
            m_HealAnimator.SetBool("IsHealPulseActive",true);
            StartCoroutine(AnimatorCooldown("IsHealPulseActive", m_HealAnimator, m_HealAnimatorCooldown));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position,m_EnergyExplosion.GetExplosionRadius());
        if (m_BoxCollider != null && transform.position != null)
        {
            Gizmos.color = Color.red;
            Vector3 rayStartPosition =
                new Vector3(transform.position.x + 0.5f * m_LastMovingDirection, transform.position.y, 0);
            Gizmos.DrawRay(rayStartPosition,new Vector3(0,-1 * m_GroundRaycastDistance,0));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,m_ObjectiveCollectRadius);
    }
    
    private void interact()
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
                case "Glide":
                    m_Glide.GetAbilityStats().SetIsUnlocked(true);
                    m_Glide.GetAbilityStats().SetIsAvailable(true);
                    break;
                case "Heal":
                    m_Heal.GetSkillsStats().SetIsUnlocked(true);
                    m_Heal.GetSkillsStats().SetIsAvailable(true);
                    break;
                case "EnergyExplosion":
                    m_EnergyExplosion.GetSkillsStats().SetIsUnlocked(true);
                    m_EnergyExplosion.GetSkillsStats().SetIsAvailable(true);
                    break;
                case "Key0":
                    m_PlayerGotKey[0] = true;
                    AudioManager.Instance.PlaySFX("Angel");
                    break;
                case "Key1":
                    m_PlayerGotKey[1] = true;
                    AudioManager.Instance.PlaySFX("Angel");
                    break;
                case "Key2":
                    m_PlayerGotKey[2] = true;
                    AudioManager.Instance.PlaySFX("Angel");
                    break;
            }
        }
    }
    private IEnumerator invicible(float i_InvicibleTime = 1f)
    {
        m_BoxCollider.isTrigger = true;
        yield return new WaitForSeconds(i_InvicibleTime);
        m_BoxCollider.isTrigger = false;
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
    public IEnumerator MovmentDisabled(float i_time)
    {
        m_movingEnabled = false;
        yield return new WaitForSeconds(i_time);

        m_movingEnabled = true;
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
    public void SetHp(float i_Hp)
    {
        m_CurrentHealthPoint += i_Hp;
        m_CurrentHealthPoint = Math.Clamp(m_CurrentHealthPoint, 0f, 100f);
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
    public bool PlayerGotKey(int i_KeyNumber)
    {
        return m_PlayerGotKey[i_KeyNumber];
    }
    public void ResetAbility(AbilityStats i_Ability)
    {
        i_Ability.SetIsUnlocked(false);
    }
    public void ResetSkill(SkillsStats i_Skill)
    {
        i_Skill.SetIsUnlocked(false);
    }

    public IEnumerator AnimatorCooldown(string i_AnimationsBool, Animator i_Animator, float i_TimeToWait)
    {
        yield return new WaitForSeconds(i_TimeToWait);
        i_Animator.SetBool(i_AnimationsBool,false);
    }
    public void GetHurtFeedback(float i_Time)
    {
        m_HitScreen.gameObject.SetActive(true);
        m_HitScreen.DOFade(0.5f, i_Time);

    }
    public IEnumerator TimeItsRed(float i_TimeItsRed, float i_TimeFadeOut)
    {
        yield return new WaitForSeconds(i_TimeItsRed);
        m_HitScreen.DOFade(0f, i_TimeFadeOut);
        yield return new WaitForSeconds(i_TimeFadeOut);
        m_HitScreen.gameObject.SetActive(false);

    }
    void Fadeout(float i_Time,GameObject i_transform)
    {
        i_transform.GetComponent<SpriteRenderer>().DOFade(0, i_Time);
    }

    public IEnumerator SetActiveToFalse(GameObject i_GameObject, float i_TimeToFalse)
    {
        yield return new WaitForSeconds(i_TimeToFalse);
        i_GameObject.SetActive(false);
    }
    
    public float WalkingSpeed
    {
        get => m_CurrentWalkingSpeed;
        set => m_CurrentWalkingSpeed = value;
    }
}
