using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Double Jump",fileName = "Double Jump")]
    public class DoubleJump : ScriptableObject
    {
        [SerializeField]
        private short k_AvailabilityLevel = 2;
        private readonly AbilityStats m_AbilityStats;
        
        public DoubleJump()
        {
            m_AbilityStats = new AbilityStats();
        }

        public void RunAbility(float i_jumpHeight,Rigidbody2D i_rigidBody)
        {
            
        }
        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }
    }

}