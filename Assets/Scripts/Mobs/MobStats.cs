using UnityEngine;

namespace Mobs
{
    public class MobStats : Component
    {
        private float m_health;
        private float m_damage;

        public MobStats(float i_health, float i_damage)
        {
            m_health = i_health;
            m_damage = i_damage;
        }

        public float GetDamage()
        {
            return m_damage;
        }

        public float getHealth()
        {
            return m_health;
        }
        
        public void GetHit(float i_damage)
        {
            m_health = Mathf.Clamp(m_health - i_damage, 0, 100);
        }

        public bool isDead()
        {
            return m_health <= 0;
        }
    }
}