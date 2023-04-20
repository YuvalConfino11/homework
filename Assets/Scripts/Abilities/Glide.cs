using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Glide",fileName = "Glide")]
    public class Glide : ScriptableObject
    {
        [SerializeField]
        private short k_AvailabilityLevel = 2;
        private readonly AbilityStats m_AbilityStats;
        
        public Glide()
        {
            m_AbilityStats = new AbilityStats(k_AvailabilityLevel);
        }

        public void RunAbility(float i_graviryFactor, Rigidbody2D i_rigidBody)
        {
                i_rigidBody.gravityScale = i_graviryFactor;
        }
        
        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }
    }

}