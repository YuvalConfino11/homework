using UnityEngine;
namespace Skills
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Heal",fileName = "Heal")]
    public class Heal : ScriptableObject
    {
        [SerializeField]
        private float m_HealAmount = 40f;
        [SerializeField] 
        private float m_HealManaPointsCost = 50f;
        private readonly SkillsStats r_SkillsStats;
        
        public Heal()
        {
            r_SkillsStats = new SkillsStats(false, true, 0);
        }
        
        public SkillsStats GetSkillsStats()
        {
            return r_SkillsStats;
        }
        
        public float GetHealAmount()
        {
            return m_HealAmount;
        }
        
        public float GetManaPointsCost()
        {
            return m_HealManaPointsCost;
        }
    }
}