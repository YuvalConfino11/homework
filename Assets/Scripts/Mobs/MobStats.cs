using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mobs
{
    public class MobStats : MonoBehaviour
    {
        [SerializeField]
        private float m_Damage = 4f;
        [FormerlySerializedAs("m_Health")] [SerializeField]
        private float m_MaxHealth = 30f;
        [FormerlySerializedAs("m_CurrentHealt")] [SerializeField]
        private float m_CurrentHealth = 100f;

        private void Awake()
        {
            m_CurrentHealth = m_MaxHealth;
        }

        public float GetDamage()
        {
            return m_Damage;
        }

        public float GetHealth()
        {
            return m_CurrentHealth;
        }
        public float GetMaxHealth()
        {
            return m_MaxHealth;
        }
        
        public void GetHit(float i_Damage)
        {
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth - i_Damage, 0, 100);
        }

        public bool isDead()
        {
            return m_CurrentHealth <= 0;
        }
    }
}