using UnityEngine;
namespace Skills
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Energy Explosion",fileName = "EnergyExplosion")]
    public class EnergyExplosion : ScriptableObject
    {
        [SerializeField]
        private short k_AvailabilityLevel = 2;
        [SerializeField]
        private float m_maxExplosionRadius = 10f;
        [SerializeField]
        private float m_ExplosionForce = 5f;
        [SerializeField] 
        private float m_ExplosionDamage = 25f;
        private readonly SkillsStats m_SkillsStats;

        public EnergyExplosion()
        {
            m_SkillsStats = new SkillsStats(k_AvailabilityLevel, false, true, 0);
        }
        
        public SkillsStats GetSkillsStats()
        {
            return m_SkillsStats;
        }
        
        public float GetExplosionRadius()
        {
            return m_maxExplosionRadius;
        }
        
        public float GetExplosionForce()
        {
           return m_ExplosionForce;
        }

        public float GetExplosionDamage()
        {
            return m_ExplosionDamage;
        }
    }
}