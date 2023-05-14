using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Skills;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private ImaginaryFriendAttack m_ImaginaryFriendAttack;
    [SerializeField]
    private float m_MoveSpeed = 200f;
    [SerializeField]
    private float m_NextWayPointDistance;
    [SerializeField]
    private LayerMask m_LayerMask;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private bool m_FriendDuringAttack = false;
    [SerializeField]
    private bool m_FriendHitMob = false;


    private Transform m_MainTarget;

    Path path;
    int m_CurrentWayPoint = 0;
    

    Seeker seeker;
    Rigidbody2D rb;
    private Collider2D m_MobInAttackRadius;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        m_MainTarget = player;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
        

    }

    void UpdatePath()     
    { 
        if(m_MobInAttackRadius != null && m_FriendDuringAttack == false)
        {
            m_MainTarget = m_MobInAttackRadius.transform;
            m_FriendDuringAttack = true;
        }
        if (m_FriendHitMob)
        {
            m_MainTarget = player;
        }
        if (Vector2.Distance(transform.position, player.position) < 1)
        {
            m_FriendDuringAttack = false;
            m_FriendHitMob = false;
        }
       
        seeker.StartPath(rb.position, m_MainTarget.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            m_CurrentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float attackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, m_LayerMask);
         if(path == null)
         {
             return;
         }
        /* if(m_MobInAttackRadius != null)
         {
             GoToTarget(m_MobInAttackRadius.transform);
         }
         else
         {
             GoToTarget(player);
         }*/

        //seeker.StartPath(rb.position, target.position, OnPathComplete);

        if (m_CurrentWayPoint >= path.vectorPath.Count)
        {
            return;
        }
        if(path.vectorPath.Count == 0)
        {
            return;
        }
        Vector2 direction = ((Vector2)path.vectorPath[m_CurrentWayPoint] - rb.position).normalized;
        Vector2 force = direction * m_MoveSpeed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[m_CurrentWayPoint]);
        if (distance < m_NextWayPointDistance)
        {
            m_CurrentWayPoint++;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mob"))
        {
            m_FriendHitMob = true;
        }
    }

    /*void GoToTarget(Transform target)
    {
        seeker.StartPath(rb.position, target.position, OnPathComplete);

        if (m_CurrentWayPoint >= path.vectorPath.Count)
        {
            m_reachedMob = true;
        }
        Vector2 direction = ((Vector2)path.vectorPath[m_CurrentWayPoint] - rb.position).normalized;
        Vector2 force = direction * m_MoveSpeed * Time.deltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[m_CurrentWayPoint]);
        if (distance < m_NextWayPointDistance)
        {
            m_CurrentWayPoint++;
        }

    }*/
}
