using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushers : MonoBehaviour
{
    [SerializeField]
    private float m_UpSpeed;
    [SerializeField]
    private float m_DownSpeed;
    [SerializeField]
    private Transform m_UpperPos;
    [SerializeField]
    private Transform m_LowerPos;
    [SerializeField]
    private ActivateCrushers m_Activate;


    private bool m_Crush;
   
    // Update is called once per frame
    void Update()
    {
 
            if (transform.position.y >= m_UpperPos.position.y && m_Activate.m_ActivateCrushers)
            {
                m_Crush = true;
            }
            if (transform.position.y <= m_LowerPos.position.y)
            {
                m_Crush = false;
            }
            if (m_Crush)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_LowerPos.position, m_DownSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, m_UpperPos.position, m_UpSpeed * Time.deltaTime);
            }
        
    }

    
   
}
