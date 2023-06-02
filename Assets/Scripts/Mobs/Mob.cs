using System.Collections;
using UnityEngine;

namespace Mobs
{
    public class Mob : MonoBehaviour
    {
        [SerializeField] 
        private float m_MobFieldOfHitRadius = 0.5f;
        [SerializeField] 
        private LayerMask m_PlayerLayerMask;
        [SerializeField] 
        private MobStats m_MobStats;
        [SerializeField]
        private GameObject m_ManaBall;
        
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
                hit();
            }
        }
        
        private void hit()
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, m_MobFieldOfHitRadius * transform.localScale.x, m_PlayerLayerMask);
            if (hitPlayer != null)
            {
                m_PlayerGameObject = hitPlayer.gameObject;
                m_PlayerGameObject.GetComponent<Player>().getHit(m_MobStats.GetDamage());
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, m_MobFieldOfHitRadius * transform.localScale.y);
        }
        
    }
}