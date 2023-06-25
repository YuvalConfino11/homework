using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    public ParallaxCamera m_ParallaxCamera;
    List<ParallaxLayer> m_ParallaxLayers = new List<ParallaxLayer>();

    void Awake()
    {
        if (m_ParallaxCamera == null)
            m_ParallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
        if (m_ParallaxCamera != null)
            m_ParallaxCamera.m_OnCameraTranslate += move;
        setLayers();
    }

    void setLayers()
    {
        m_ParallaxLayers.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                layer.name = "Layer-" + i;
                m_ParallaxLayers.Add(layer);
            }
        }
    }
    void move(float i_Delta)
    {
        foreach (ParallaxLayer layer in m_ParallaxLayers)
        {
            layer.Move(i_Delta);
        }
    }
}
