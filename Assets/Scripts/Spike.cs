using System;
using UnityEngine;
using Mobs;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private Spikes m_Spikes;
    
    private void Update()
    {
        if (m_Spikes.SpikeHP <= 0)
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
}
