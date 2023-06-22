using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float m_SpawnInterval = 5f;
    [SerializeField]
    private float m_SpawnManagerRadius = 20f;
    [SerializeField]
    private bool m_IsPlayerInRadius = false;
    [SerializeField]
    private LayerMask m_PlayerLayerMask;
    [SerializeField]
    private LayerMask m_MobsLayerMask;
    [SerializeField]
    private List<GameObject> m_MobsList;
    [SerializeField]
    private int m_MaxMobsInArea = 10;
    
    
    private bool m_IsFirstEnter = true;
    private List<GameObject> m_SpwanPointsList;
    private bool m_IsKillMob = false;
    private int m_MobsCounter = 0;
    private int m_LastMobsCounter = 0;
    private float m_SpawnTimer = 0f;

    private void Start()
    {
        m_SpwanPointsList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (!child.gameObject.CompareTag("Ground"))
            {
                m_SpwanPointsList.Add(child.gameObject);
            }
        }
    }
    
    private void Update()
    {
        m_IsPlayerInRadius = Physics2D.OverlapCircle(transform.position, m_SpawnManagerRadius, m_PlayerLayerMask) != null;
        GameObject randomMob = m_MobsList[(int)Random.Range(0, m_MobsList.Count)];
        GameObject randomSpawnPoint = m_SpwanPointsList[(int)Random.Range(0, m_SpwanPointsList.Count)];
        float randomMobHeight = Mathf.Abs(randomMob.gameObject.GetComponentInChildren<BoxCollider2D>().offset.y);
        Vector3 randomSpawnPointPosition = new Vector3(randomSpawnPoint.transform.position.x, randomSpawnPoint.transform.position.y + randomMobHeight, randomSpawnPoint.transform.position.z);
        if (m_IsFirstEnter && m_IsPlayerInRadius)
        {
            Instantiate(randomMob, randomSpawnPointPosition, Quaternion.identity);
            m_LastMobsCounter = Physics2D.OverlapCircleAll(transform.position, m_SpawnManagerRadius,m_MobsLayerMask).Length;
            m_SpawnTimer = 0f;
            m_IsFirstEnter = false;
            m_IsKillMob = false;
        }
        else
        {
            if (m_IsPlayerInRadius && m_SpawnTimer >= m_SpawnInterval && m_MobsCounter < m_MaxMobsInArea)
            {
                Instantiate(randomMob, randomSpawnPointPosition, Quaternion.identity);
                m_LastMobsCounter = Physics2D.OverlapCircleAll(transform.position, m_SpawnManagerRadius,m_MobsLayerMask).Length;
                m_SpawnTimer = 0f;
                m_IsKillMob = false;
            }
        }
        m_SpawnTimer += Time.deltaTime;
        m_MobsCounter = Physics2D.OverlapCircleAll(transform.position, m_SpawnManagerRadius,m_MobsLayerMask).Length;
        if (m_MobsCounter < m_LastMobsCounter && !m_IsKillMob)
        {
            m_SpawnTimer = 0f;
            m_IsKillMob = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(141, 191, 144);
        Gizmos.DrawWireSphere(transform.position, m_SpawnManagerRadius);
        if (m_SpwanPointsList != null)
        {
            foreach (GameObject child in m_SpwanPointsList)
            {
                if (child != null)
                {
                    Gizmos.DrawWireSphere(child.transform.position, 10f);
                }
            }
        }
        
    }
}
