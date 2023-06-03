using System;
using UnityEngine;
using UnityEngine.U2D;

public class BackgroundColorController : MonoBehaviour
{
    private SpriteRenderer m_BackgroundSpriteRenderer;
    private SpriteShapeRenderer m_BackgroundSpriteShapeRenderer;
    private float m_MaxHealth;
    [SerializeField]
    private Player m_Player;


    // Start is called before the first frame update
    void Start()
    {
        m_MaxHealth = m_Player.GetMaxHealth();
        TryGetComponent<SpriteRenderer>(out m_BackgroundSpriteRenderer);
        TryGetComponent<SpriteShapeRenderer>(out m_BackgroundSpriteShapeRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        float healthRatio = Mathf.Clamp(((m_Player.GetCurrentHealth() / m_MaxHealth) + 0.05f),0,1);
        Color targetColor = Color.HSVToRGB(0,  0, healthRatio, false);
        if (m_BackgroundSpriteRenderer != null)
        {
            m_BackgroundSpriteRenderer.color = Color.Lerp(m_BackgroundSpriteRenderer.color, targetColor, Time.deltaTime);
        }
        else if (m_BackgroundSpriteShapeRenderer != null)
        {
            m_BackgroundSpriteShapeRenderer.color = Color.Lerp(m_BackgroundSpriteShapeRenderer.color, targetColor, Time.deltaTime);
        }
        
    }
}