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
        private float m_BallFallRatio = 0.3f;
        [SerializeField]
        private Rigidbody2D m_RigidBody;
        [SerializeField]
        private float m_KnockbackPower = 15;
        [SerializeField]
        private float m_KnockbackDuration = 0.2f;
        [SerializeField] 
        private MobAnimation m_MobAnimation;

        
        private SpriteRenderer m_SpriteRenderer;
        private Color m_MobColor;
        
        private float timer = 0;

        private void Awake()
        {
            m_MobStats = GetComponent<MobStats>();
            m_SpriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
            m_RigidBody = transform.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            m_MobColor = m_SpriteRenderer.color;
            m_MobColor.a = Mathf.Clamp(m_MobStats.GetHealth() / m_MobStats.GetMaxHealth(),0.5f,1f);
            m_SpriteRenderer.color = m_MobColor;
            timer += Time.deltaTime;
            if (timer >= m_MobHitCooldown)
            {
                m_CanHitPlayer = true;
                timer = 0;
            }
            if (m_MobStats.isDead())
            {
                float randomValue = UnityEngine.Random.Range(0f, 1f);
                if (randomValue > m_BallFallRatio)
                {
                    Instantiate(ChooseBall(m_ManaBall, m_HPBall), transform.position, transform.rotation);
                }
                Destroy(this.gameObject);
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
            float randomValue = UnityEngine.Random.Range(0f,1f);
            if(randomValue > m_BallFallRatio)
            {
                return i_ManaBall;
            }
            return i_HPBall;
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