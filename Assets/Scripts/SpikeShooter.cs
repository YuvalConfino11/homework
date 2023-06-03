using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Spike;
    [SerializeField]
    private Transform m_SpikePos;
    [SerializeField]
    private float m_TimeBetweenSpikes = 0.8f;

    private float m_Timer;
    
    void Update()
    {
        m_Timer += Time.deltaTime;
        if(m_Timer > m_TimeBetweenSpikes)
        {
            m_Timer = 0;
        }
    }
    
}
