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

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > m_TimeBetweenSpikes)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(m_Spike, m_SpikePos.position, Quaternion.identity);
    }

   
}
