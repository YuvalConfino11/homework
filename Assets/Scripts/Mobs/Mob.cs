using System;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    public class Mob : MonoBehaviour
    {
        [SerializeField] 
        private MobStats m_MobStats;
        [SerializeField]
        private GameObject m_ManaBall;
        [SerializeField]
        bool m_CanHitPlayer = false;
        [SerializeField]
        private float m_MobHitCooldown = 0.75f;
        
        // private GameObject m_PlayerGameObject;
        private float timer = 0;

        private void Awake()
        {
            m_MobStats = GetComponent<MobStats>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= m_MobHitCooldown)
            {
                m_CanHitPlayer = true;
                timer = 0;
            }
            if (m_MobStats.isDead())
            {
                Destroy(this.gameObject);
                Instantiate(m_ManaBall, transform.position, transform.rotation);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            
            if (col.gameObject.CompareTag("Player") && m_CanHitPlayer)
            {
                Player player = col.gameObject.GetComponent<Player>();
                player.getHit(m_MobStats.GetDamage());
                m_CanHitPlayer = false;
            }
        }
    }
}