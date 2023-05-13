using UnityEngine;
using Skills;

public class ImaginaryFriend : MonoBehaviour
{
    [SerializeField]
    private ImaginaryFriendAttack m_ImaginaryFriendAttack;
    [SerializeField]
    private float m_MoveSpeedTowardMob = 1f;
    [SerializeField]
    private float m_MoveSpeedTowardPlayer = 1f;
    [SerializeField]
    private bool m_FriendDuringAttack = false;
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private GameObject m_StartingFriendPos;
    [SerializeField]
    private bool m_FriendHitMob = false;


    private Collider2D m_MobInAttackRadius;


    void Update()
    {
        float attackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
        m_LayerMask = LayerMask.GetMask("Mob");
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, m_LayerMask);
        
        if (m_FriendDuringAttack == false)
        {
            Vector2.MoveTowards(transform.position, m_StartingFriendPos.transform.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
        }

        if (m_MobInAttackRadius != null)
        {
            
            if (Vector2.Distance(transform.position, m_StartingFriendPos.transform.position) == 0 && m_FriendDuringAttack == false)
            {
                m_FriendDuringAttack = true;
                transform.position = Vector2.MoveTowards(transform.position, m_MobInAttackRadius.transform.position, m_MoveSpeedTowardMob * Time.deltaTime);
            }
            if(m_FriendDuringAttack && Vector2.Distance(transform.position, m_StartingFriendPos.transform.position) != 0 && !m_FriendHitMob)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_MobInAttackRadius.transform.position, m_MoveSpeedTowardMob * Time.deltaTime);
            }
            else
            {
                m_FriendDuringAttack = false;
                if (Vector2.Distance(transform.position, m_StartingFriendPos.transform.position) != 0)
                {
                   transform.position = Vector2.MoveTowards(transform.position, m_StartingFriendPos.transform.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
                }
                if (Vector2.Distance(transform.position, m_StartingFriendPos.transform.position) < 0.01)
                {
                    transform.position = m_StartingFriendPos.transform.position;
                    m_FriendHitMob = false;
                    m_FriendDuringAttack = true;
                }
            }
          
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, m_StartingFriendPos.transform.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, m_ImaginaryFriendAttack.GetAttackRadius());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            m_FriendHitMob = true;
        }
    }
}
