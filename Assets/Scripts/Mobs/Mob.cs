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
        private Transform m_CastPosition;

        private void Awake()
        {
            StartCoroutine(playerHitCheck());
            m_MobStats = GetComponent<MobStats>();
            m_CastPosition = transform.GetChild(0).transform;
            
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
            Collider2D hitPlayer = Physics2D.OverlapCircle(m_CastPosition.position, m_MobFieldOfHitRadius, m_PlayerLayerMask);
            if (hitPlayer != null)
            {
                m_PlayerGameObject = hitPlayer.gameObject;
                m_PlayerGameObject.GetComponent<Player>().getHit(m_MobStats.GetDamage());
            }
        }
        
    }
}