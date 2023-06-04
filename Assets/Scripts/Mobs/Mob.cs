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
        
        private GameObject m_PlayerGameObject;

        private void Awake()
        {
            StartCoroutine(playerHitCheck());
            m_MobStats = GetComponent<MobStats>();
            
        }

        private void Update()
        {
            if (m_MobStats.isDead())
            {
                Destroy(this.gameObject);
                Instantiate(m_ManaBall, transform.position, transform.rotation);
            }
        }

        private IEnumerator playerHitCheck()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                yield return wait;
                m_CanHitPlayer = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player") && m_CanHitPlayer)
            {
                col.gameObject.GetComponent<Player>().getHit(m_MobStats.GetDamage());
                m_CanHitPlayer = false;
            }
        }
    }
}