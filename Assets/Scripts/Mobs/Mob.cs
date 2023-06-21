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
        private GameObject m_HPBall;
        [SerializeField]
        bool m_CanHitPlayer = false;
        [SerializeField]
        private float m_MobHitCooldown = 0.75f;
        [SerializeField]
        private float m_BallFallRatio = 0.7f;
        [SerializeField]
        private Rigidbody2D m_RigidBody;
        [SerializeField]
        private float m_KnockbackPower = 15;
        [SerializeField]
        private float m_KnockbackDuration = 0.2f;

        // private GameObject m_PlayerGameObject;
        private float timer = 0;

        private void Awake()
        {
            m_MobStats = GetComponent<MobStats>();
            m_RigidBody = transform.GetComponent<Rigidbody2D>();
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
                Instantiate(ChooseBall(m_ManaBall , m_HPBall), transform.position, transform.rotation);
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
            if (col.gameObject.CompareTag("Bullet"))
            {
                StartCoroutine(EnemyKnockback(m_KnockbackDuration, m_KnockbackPower, col.transform));
            }
        }
        private GameObject ChooseBall(GameObject i_ManaBall , GameObject i_HPBall)
        {
            System.Random random = new System.Random();
            float randomValue = random.Next(0,1);
            if(randomValue > m_BallFallRatio)
            {
                return i_ManaBall;
            }
            else
            {
                return i_HPBall;
            }
            
        }
        public IEnumerator EnemyKnockback(float i_KnockbackDuration, float i_KnockbackPower, Transform i_ObjectTransform)
        {
            float timer = 0;
           

            while (i_KnockbackDuration > timer)
            {
                timer += Time.deltaTime;
                Vector2 dir = new Vector2(i_ObjectTransform.transform.position.x - transform.position.x, 0);
                m_RigidBody.AddForce(-dir * i_KnockbackPower);
            }

            yield return 0;
        }
    }
}