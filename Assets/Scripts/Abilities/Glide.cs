using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Glide",fileName = "Glide")]
    public class Glide : ScriptableObject
    {
        [SerializeField]
        private short k_AvailabilityLevel = 2;
        [SerializeField]
        private float m_glideFactor = 0.09f;
        private readonly AbilityStats m_AbilityStats;
        
        public Glide()
        {
            m_AbilityStats = new AbilityStats(k_AvailabilityLevel);
        }
        
        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }

        public float GetGlideFactor()
        {
            return m_glideFactor;
        }
    }

}