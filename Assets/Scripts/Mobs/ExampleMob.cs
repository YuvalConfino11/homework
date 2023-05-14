using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mobs
{
    public class ExampleMob : MonoBehaviour
    {
        [SerializeField]
        private float m_Damage = 4f;
        [SerializeField]
        private float m_Health = 30f;
        [SerializeField] 
        private float m_MobFieldOfHitRadius = 1f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        
        
        private MobStats m_mobStats;
        private GameObject m_PlayerGameObject;

        private void Awake()
        {
            StartCoroutine(playerHitCheck());
        }

        private void Update()
        {
            
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
                m_PlayerGameObject.GetComponent<Player>().getHit(m_Damage);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, m_MobFieldOfHitRadius * transform.localScale.y);
        }
        
    }
}