using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float i_DeltaMovement);
    public ParallaxCameraDelegate m_OnCameraTranslate;
    private float m_OldPosition;
    void Start()
    {
        m_OldPosition = transform.position.x;
    }
    void FixedUpdate()
    {
        if (transform.position.x != m_OldPosition)
        {
            if (m_OnCameraTranslate != null)
            {
                float delta = m_OldPosition - transform.position.x;
                m_OnCameraTranslate(delta);
            }
            m_OldPosition = transform.position.x;
        }
    }
}
