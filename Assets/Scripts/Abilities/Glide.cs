using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Glide",fileName = "Glide")]
    public class Glide : ScriptableObject
    {
        [SerializeField]
        private float m_GlideFactor = 0.09f;
        private readonly AbilityStats r_AbilityStats;
        
        public Glide()
        {
            r_AbilityStats = new AbilityStats();
        }
        
        public AbilityStats GetAbilityStats()
        {
            return r_AbilityStats;
        }

        public float GetGlideFactor()
        {
            return m_GlideFactor;
        }
    }

}