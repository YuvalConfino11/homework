using System;
using UnityEngine;
using UnityEngine.U2D;

public class BackgroundColorController : MonoBehaviour
{
    private SpriteRenderer m_backgroundSpriteRenderer;
    private SpriteShapeRenderer m_backgroundSpriteShapeRenderer;
    private float m_maxHealth;
    [SerializeField]
    private Player m_player;


    // Start is called before the first frame update
    void Start()
    {
        m_maxHealth = m_player.GetMaxHealth();
        TryGetComponent<SpriteRenderer>(out m_backgroundSpriteRenderer);
        TryGetComponent<SpriteShapeRenderer>(out m_backgroundSpriteShapeRenderer);
    }

    // Update is called once per frame
    void Update()
    {
        float healthRatio = Mathf.Clamp(((m_player.GetCurrentHealth() / m_maxHealth) + 0.2f),0,1);
        Color targetColor = Color.HSVToRGB(0,  0, healthRatio, false);
        if (m_backgroundSpriteRenderer != null)
        {
            m_backgroundSpriteRenderer.color = Color.Lerp(m_backgroundSpriteRenderer.color, targetColor, Time.deltaTime);
        }
        else if (m_backgroundSpriteShapeRenderer != null)
        {
            m_backgroundSpriteShapeRenderer.color = Color.Lerp(m_backgroundSpriteShapeRenderer.color, targetColor, Time.deltaTime);
        }
        
    }
}