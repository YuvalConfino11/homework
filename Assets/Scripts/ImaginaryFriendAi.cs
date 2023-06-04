using System;
using System.Collections;
using Mobs;
using UnityEngine;
using Pathfinding;
using Skills;

public class ImaginaryFriendAi : MonoBehaviour
{
    [SerializeField]
    private ImaginaryFriendAttack m_ImaginaryFriendAttack;
    [SerializeField]
    private LayerMask m_MobLayerMask;
    [SerializeField]
    private bool m_FriendDuringAttack = false;
    [SerializeField]
    private bool m_FriendHitMob = false;
    [SerializeField]
    private float m_MoveSpeedTowardMob = 12f; 
    [SerializeField]
    private float m_MoveSpeedTowardPlayer = 12f;
    [SerializeField]
    private float m_WaitForAttack = 1.5f;
    
    private Transform m_MainTarget;
    private Path m_Path;
    private Collider2D m_MobInAttackRadius;
    private Transform m_ImaginaryFriendStartPosition;
    private float m_AttackRadius;
    
    
    void Start()
    {
        m_ImaginaryFriendStartPosition = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
        m_AttackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
    }
    
    void Update()
    {
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, m_AttackRadius, m_MobLayerMask);
        
        if (m_MobInAttackRadius != null)
        {
            if (!m_FriendDuringAttack || (m_FriendDuringAttack && !m_FriendHitMob))
            {
                Debug.Log(m_FriendDuringAttack);
                attack();
            } 
            else
            {
                returnToPlayer();
            }
        }
        else if (m_MobInAttackRadius == null)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_ImaginaryFriendStartPosition.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, m_ImaginaryFriendStartPosition.position) < 0.01)
        {
            transform.position = m_ImaginaryFriendStartPosition.position;
            StartCoroutine(WaitForAttack(m_WaitForAttack));
            m_FriendHitMob = false;
        }
    }

    private void attack()
    {
        m_FriendDuringAttack = true;
        transform.position = Vector2.MoveTowards(transform.position, m_MobInAttackRadius.transform.position, m_MoveSpeedTowardMob * Time.deltaTime);
    }

    private void returnToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_ImaginaryFriendStartPosition.position, m_MoveSpeedTowardPlayer * Time.deltaTime);
    }
    
    
    private void OnTriggerEnter2D(Collider2D i_Collision)
    {
        if (i_Collision.gameObject.CompareTag("Mob"))
        {
            m_FriendHitMob = true;
            i_Collision.gameObject.GetComponent<MobStats>().GetHit(m_ImaginaryFriendAttack.getAttackDamage());
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position,m_AttackRadius);
    }
    public IEnumerator WaitForAttack(float i_TimerForAttack)
    {
        m_FriendDuringAttack = true;
        yield return new WaitForSeconds(i_TimerForAttack);
        m_FriendDuringAttack = false;
    }
}
