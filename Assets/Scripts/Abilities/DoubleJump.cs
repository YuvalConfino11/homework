using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Double Jump",fileName = "Double Jump")]
    public class DoubleJump : ScriptableObject
    {
        private readonly AbilityStats r_AbilityStats;
        
        public DoubleJump()
        {
            r_AbilityStats = new AbilityStats();
        }

        public void RunAbility(float i_JumpHeight,Rigidbody2D i_RigidBody)
        {
            
        }
        public AbilityStats GetAbilityStats()
        {
            return r_AbilityStats;
        }
    }

}