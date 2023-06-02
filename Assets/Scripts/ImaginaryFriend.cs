using System;
using Mobs;
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
    private LayerMask m_MobLayerMask;
    [SerializeField]
    private bool m_FriendHitMob = false;

    private GameObject m_StartingFriendPos;
    private Collider2D m_MobInAttackRadius;

    private void Awake()
    {
        m_StartingFriendPos = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
    }

    void Update()
    {
        float attackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, m_MobLayerMask);
       
        if (m_MobInAttackRadius != null)
        {
            if (!m_FriendDuringAttack || (m_FriendDuringAttack && !m_FriendHitMob))
            {
                attack();
            }
            else
            {
                returnToPlayer();
            }
        }
        else if (m_MobInAttackRadius == null)
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

    private void attack()
    {
        m_FriendDuringAttack = true;
        transform.position = Vector2.MoveTowards(transform.position, m_MobInAttackRadius.transform.position, m_MoveSpeedTowardMob * Time.deltaTime);
    }

    private void returnToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_StartingFriendPos.transform.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
    }
    
    

    
}
