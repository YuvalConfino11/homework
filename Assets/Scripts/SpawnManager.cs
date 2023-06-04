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
    private int m_MobsCounter = 0;
    private float m_SpawnTimer = 0f;

    private void Start()
    {
        m_SpwanPointsList = new List<GameObject>();
        foreach (Transform child in transform)
        {
            m_SpwanPointsList.Add(child.gameObject);
        }
    }
    
    private void Update()
    {
        m_IsPlayerInRadius = Physics2D.OverlapCircle(transform.position, m_SpawnManagerRadius, m_PlayerLayerMask) != null;
        if (m_IsFirstEnter && m_IsPlayerInRadius)
        {
            GameObject randomMob = m_MobsList[(int)Random.Range(0, m_MobsList.Count)];
            GameObject randomSpawnPoint = m_SpwanPointsList[(int)Random.Range(0, m_SpwanPointsList.Count)];
            Instantiate(randomMob, randomSpawnPoint.transform.position, Quaternion.identity);
            m_SpawnTimer = 0f;
            m_IsFirstEnter = false;
        }
        else
        {
            if (m_IsPlayerInRadius && m_SpawnTimer >= m_SpawnInterval && m_MobsCounter < m_MaxMobsInArea)
            {
                GameObject randomMob = m_MobsList[(int)Random.Range(0, m_MobsList.Count)];
                GameObject randomSpawnPoint = m_SpwanPointsList[(int)Random.Range(0, m_SpwanPointsList.Count)];
                Instantiate(randomMob, randomSpawnPoint.transform.position, Quaternion.identity);
                m_SpawnTimer = 0f;
            }
        }
        m_SpawnTimer += Time.deltaTime;
        m_MobsCounter = Physics2D.OverlapCircleAll(transform.position, m_SpawnManagerRadius,m_MobsLayerMask).Length;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(141, 191, 144);
        Gizmos.DrawWireSphere(transform.position, m_SpawnManagerRadius);
        foreach (Transform child in transform)
        {
            Gizmos.DrawWireSphere(child.position, 10f);
        }
    }
}
