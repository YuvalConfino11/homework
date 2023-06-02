using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dash",fileName = "Dash")]
    public class Dash : ScriptableObject
    {
        [SerializeField]
        private float k_CooldownTime = 1f;
        [SerializeField]
        private float m_DashSpeed = 10f;
        private readonly AbilityStats m_AbilityStats;
        
        public Dash()
        {
            m_AbilityStats = new AbilityStats(false,true,k_CooldownTime);
        }

        public AbilityStats GetAbilityStats()
        {
            return m_AbilityStats;
        }

        public float GetDashSpeed()
        {
            return m_DashSpeed;
        }
    }
}