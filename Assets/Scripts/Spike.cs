using System;
using UnityEngine;
using Mobs;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private Spikes m_Spikes;
    [SerializeField]
    private float m_currentHealth;

    private void Awake()
    {
        m_currentHealth = m_Spikes.SpikeHP;
    }

    private void Update()
    {
        if (m_currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().getHit(m_Spikes.SpikeDamage);
        }
    }
    
    public void GetHit(float i_Damage)
    {
        m_currentHealth = Mathf.Clamp(m_currentHealth - i_Damage, 0, 100);
    }
}
