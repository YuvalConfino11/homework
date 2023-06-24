using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSpawnManager : MonoBehaviour
{
    [SerializeField]
    private float m_SpawnManagerRadius = 20f;
    [SerializeField]
    private bool m_IsPlayerInRadius = false;
    [SerializeField]
    private LayerMask m_PlayerLayerMask;
    [SerializeField]
    private LayerMask m_MobsLayerMask;
    [SerializeField]
    private GameObject m_BossMob;
    [SerializeField]
    private float m_MaxBarriersMovingDistance = 60f;
    [SerializeField]
    private float m_BarriersMovingSpeed = 10f;
    
    
    private bool m_IsFirstEnter = true;
    private GameObject m_SpwanPoint;
    private bool m_IsMobDead = false;
    private  float m_MobHeight;
    private List<GameObject> m_BossAreaBarriers = new List<GameObject>();
    private bool m_IsBarrierUp = false;
    private bool m_IsMoveBarrier = false;
    private float m_BarrierMovingDirection = 1f;
    private float m_BarriersMovingDistance = 0;
    private float m_YScale = 3f;
    private bool m_InBoss = false;
    

    private void Start()
    {
        m_SpwanPoint = transform.GetChild(0).gameObject;
        m_MobHeight= Mathf.Abs(m_BossMob.gameObject.GetComponentInChildren<BoxCollider2D>().offset.y);
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Ground"))
            {
                m_BossAreaBarriers.Add(child.gameObject);
            }
        }
    }
    
    private void Update()
    {
        if (m_IsMoveBarrier)
        {
            if (Mathf.Abs(m_BarriersMovingDistance) < Mathf.Abs(m_MaxBarriersMovingDistance))
            {
                foreach (GameObject barrier in m_BossAreaBarriers)
                {
                    barrier.transform.position += new Vector3(0, m_BarrierMovingDirection * Time.deltaTime * m_BarriersMovingSpeed, 0);
                    barrier.transform.localScale = new Vector3(barrier.transform.localScale.x, m_YScale, barrier.transform.localScale.z);
                }
                if (!m_InBoss)
                {
                    AudioManager.Instance.PlayMusic("Boss ver1");
                    m_InBoss = true;
                }
                
                m_BarriersMovingDistance += m_BarrierMovingDirection * Time.deltaTime * m_BarriersMovingSpeed;
            }
            else
            {
                m_IsBarrierUp = !m_IsBarrierUp;
                m_IsMoveBarrier = false;
            }
        }
        
        m_IsPlayerInRadius = Physics2D.OverlapBox(transform.position, new Vector2(2 * m_SpawnManagerRadius,m_SpawnManagerRadius), 0,m_PlayerLayerMask) != null;
        if (m_IsFirstEnter && m_IsPlayerInRadius)
        {
            Vector3 spawnPointPosition = new Vector3(m_SpwanPoint.transform.position.x, m_SpwanPoint.transform.position.y + m_MobHeight, m_SpwanPoint.transform.position.z);
            Instantiate(m_BossMob, spawnPointPosition, Quaternion.identity);
            m_IsFirstEnter = false;
            m_IsMobDead = false;
            foreach (GameObject barrier in m_BossAreaBarriers)
            {
                barrier.transform.position += new Vector3(0, m_MaxBarriersMovingDistance * Time.deltaTime, 0);
            }
            m_BarrierMovingDirection = 1f;
            m_IsMoveBarrier = true;
        }

        if (m_IsMobDead && m_IsBarrierUp && !m_IsFirstEnter && !m_IsMoveBarrier)
        {
            m_YScale = 1f;
            m_BarriersMovingDistance = 0;
            m_BarrierMovingDirection = -1f;
            m_IsMoveBarrier = true;
            if (m_InBoss)
            {
                AudioManager.Instance.PlayMusic("Happy ver1");
            }
        }

        m_IsMobDead = Physics2D.OverlapBox(transform.position, new Vector2(m_SpawnManagerRadius,m_SpawnManagerRadius),0, m_MobsLayerMask) == null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(141, 191, 144);
        Vector2 areaCenter = new Vector2(transform.position.x, 1.5f * transform.position.y);
        Gizmos.DrawWireCube(transform.position, new Vector3(2 * m_SpawnManagerRadius,m_SpawnManagerRadius, 0));
        if (m_SpwanPoint != null)
        {
            Gizmos.DrawWireSphere(m_SpwanPoint.transform.position, 10f);
        }
    }

}
