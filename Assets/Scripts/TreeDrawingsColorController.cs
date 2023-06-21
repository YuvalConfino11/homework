using System;
using UnityEngine;
using UnityEngine.U2D;

public class TreeDrawingsColorController : MonoBehaviour
{
    private SpriteRenderer m_DrawingSpriteRenderer;
    private float m_MaxHealth;
    [SerializeField]
    private Player m_Player;


    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_MaxHealth = m_Player.GetMaxHealth();
        TryGetComponent<SpriteRenderer>(out m_DrawingSpriteRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        Color targetColor = m_DrawingSpriteRenderer.color;
        if ((m_Player.GetCurrentHealth() / m_MaxHealth) > 0.6f)
        {
            targetColor.a = 0f;
        }
        else
        {
            float healthRatio = (1 - ((m_Player.GetCurrentHealth() / m_MaxHealth)/0.6f));
            targetColor.a = healthRatio;   
        }
        m_DrawingSpriteRenderer.color = targetColor;
    }
}