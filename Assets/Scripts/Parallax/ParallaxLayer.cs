using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    private float m_StartPos, m_LengthSprite;
    public float m_ParallaxFactor;

    private void Start()
    {
        m_StartPos = transform.position.x;
        m_LengthSprite = GetComponent<SpriteRenderer>().bounds.size.x;
    }
    public void Move(float i_Delta)
    {

        Vector3 newPos = transform.localPosition;
        newPos.x -= i_Delta * m_ParallaxFactor;
        transform.localPosition = newPos;
    }
}
