using System.Threading.Tasks;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dash",fileName = "Dash")]
    public class Dash : ScriptableObject
    {
        [SerializeField]
        private short k_AvailabilityLevel = 2;
        [SerializeField]
        private float k_CooldownTime = 1f;
        private readonly AbilityStats m_AbilityStats;
        
        public Dash()
        {
            m_AbilityStats = new AbilityStats(k_AvailabilityLevel,false,true,k_CooldownTime);
        }

        public void RunAbility(float i_movingDirection, float i_dashSpeed, Rigidbody2D i_Rigidbody2D)
        {
            m_AbilityStats.SetIsAvailable(false);
            i_Rigidbody2D.AddForce(new Vector2(i_movingDirection * i_dashSpeed, 0), ForceMode2D.Impulse);
        }

        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }
        
    }
}