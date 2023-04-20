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
            m_AbilityStats = new AbilityStats(k_AvailabilityLevel);
        }

        public void RunAbility(float i_jumpHeight,Rigidbody2D i_rigidBody)
        {
            float jumpForce = Mathf.Sqrt( -2 * i_jumpHeight * (Physics2D.gravity.y * i_rigidBody.gravityScale));
            i_rigidBody.velocity = Vector2.up * jumpForce;
            // i_rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            m_AbilityStats.SetIsAvailable(false);
        }
        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }
        
    }

}