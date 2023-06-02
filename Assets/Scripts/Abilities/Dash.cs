using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Dash",fileName = "Dash")]
    public class Dash : ScriptableObject
    {
        [SerializeField]
        private float m_CooldownTime = 1f;
        [SerializeField]
        private float m_DashSpeed = 10f;
        private readonly AbilityStats r_AbilityStats;
        
        public Dash()
        {
            r_AbilityStats = new AbilityStats(false,true,m_CooldownTime);
        }

        public AbilityStats GetAbilityStats()
        {
            return r_AbilityStats;
        }

        public float GetDashSpeed()
        {
            return m_DashSpeed;
        }
    }
}