using UnityEngine;

namespace Mobs
{
    public class MobStats : MonoBehaviour
    {
        [SerializeField]
        private float m_Damage = 4f;
        [SerializeField]
        private float m_Health = 30f;
        
        public float GetDamage()
        {
            return m_Damage;
        }

        public float getHealth()
        {
            return m_Health;
        }
        
        public void GetHit(float i_damage)
        {
            Debug.Log(1);
            m_Health = Mathf.Clamp(m_Health - i_damage, 0, 100);
        }

        public bool isDead()
        {
            return m_Health <= 0;
        }
    }
}