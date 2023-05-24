using UnityEngine;
using Pathfinding;
using Skills;

public class ImaginaryFriendAi : MonoBehaviour
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
    private bool m_FriendDuringAttack = false;
    [SerializeField]
    private bool m_FriendHitMob = false;
    
    private Transform m_MainTarget;
    private Path m_Path;
    private int m_CurrentWayPoint = 0;
    private Seeker m_Seeker;
    private Rigidbody2D m_Rigidbody2D;
    private Collider2D m_MobInAttackRadius;
    private Transform m_ImaginaryFriendStartPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_Seeker = GetComponent<Seeker>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_ImaginaryFriendStartPosition = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
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
            m_MainTarget = m_ImaginaryFriendStartPosition;
        }
        if (Vector2.Distance(transform.position, m_ImaginaryFriendStartPosition.position) < 1)
        {
            m_FriendDuringAttack = false;
            m_FriendHitMob = false;
        }
        if (m_MobInAttackRadius == null)
        {
            m_MainTarget = m_ImaginaryFriendStartPosition;
        }
        m_Seeker.StartPath(m_Rigidbody2D.position, m_MainTarget.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            m_Path = p;
            m_CurrentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float attackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, m_LayerMask);
         if(m_Path == null)
         {
             return;
         }

         if (m_CurrentWayPoint >= m_Path.vectorPath.Count)
        {
            return;
        }
        if(m_Path.vectorPath.Count == 0)
        {
            return;
        }
        Vector2 direction = ((Vector2)m_Path.vectorPath[m_CurrentWayPoint] - m_Rigidbody2D.position).normalized;
        Vector2 force = direction * m_MoveSpeed * Time.deltaTime;
        m_Rigidbody2D.AddForce(force);

        float distance = Vector2.Distance(m_Rigidbody2D.position, m_Path.vectorPath[m_CurrentWayPoint]);
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
}
