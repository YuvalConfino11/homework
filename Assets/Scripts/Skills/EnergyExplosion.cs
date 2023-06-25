using UnityEngine;
namespace Skills
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Energy Explosion",fileName = "EnergyExplosion")]
    public class EnergyExplosion : ScriptableObject
    {
        [SerializeField]
        private float m_MaxExplosionRadius = 10f;
        [SerializeField]
        private float m_ExplosionForce = 5f;
        [SerializeField] 
        private float m_ExplosionDamage = 25f;
        [SerializeField] 
        private float m_ExplosionManaPointsCost = 25f;
        private readonly SkillsStats r_SkillsStats;

        public EnergyExplosion()
        {
            r_SkillsStats = new SkillsStats(false, true, 0);
        }
        
        public SkillsStats GetSkillsStats()
        {
            return r_SkillsStats;
        }
        
        public float GetExplosionRadius()
        {
            return m_MaxExplosionRadius;
        }
        
        public float GetExplosionForce()
        {
           return m_ExplosionForce;
        }

        public float GetExplosionDamage()
        {
            return m_ExplosionDamage;
        }
        
        public float getExplosionManaPoints()
        {
            return m_ExplosionManaPointsCost;
        }
    }
}