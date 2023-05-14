using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mobs
{
    public class ExampleMob : MonoBehaviour
    {
        [SerializeField] 
        private float m_MobFieldOfHitRadius = 1f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        [SerializeField] 
        private MobStats m_mobStats;
        
        private GameObject m_PlayerGameObject;

        private void Awake()
        {
            StartCoroutine(playerHitCheck());
            m_mobStats = GetComponent<MobStats>();
        }

        private void Update()
        {
            if (m_mobStats.isDead())
            {
                Destroy(this.gameObject);
            }
        }

        private IEnumerator playerHitCheck()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                yield return wait;
                hit();
            }
        }
        
        private void hit()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, m_MobFieldOfHitRadius * transform.localScale.x, m_PlayerLayerMask);
            if (hitPlayer != null)
            {
                m_PlayerGameObject = hitPlayer.gameObject;
                m_PlayerGameObject.GetComponent<Player>().getHit(m_mobStats.GetDamage());
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, m_MobFieldOfHitRadius * transform.localScale.y);
        }
        
    }
}