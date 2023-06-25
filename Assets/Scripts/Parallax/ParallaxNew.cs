using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxNew : MonoBehaviour
{
    private float m_LengthSpriteX, m_StartPosX;
    public GameObject m_Cam;
    public float m_ParallaxEffectX;

    private void Start()
    {
        m_StartPosX = transform.position.x;
        m_LengthSpriteX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float tempX = m_Cam.transform.position.x * (1 - m_ParallaxEffectX);
        float distanceX = m_Cam.transform.position.x * m_ParallaxEffectX;
        transform.position = new Vector3(m_StartPosX + distanceX, transform.position.y, transform.position.z);
        if (tempX > m_StartPosX + m_LengthSpriteX)
        {
            m_StartPosX += m_LengthSpriteX;
        }
        else if (tempX < m_StartPosX - m_LengthSpriteX)
        {
            m_StartPosX -= m_LengthSpriteX;
        }
    }
}
