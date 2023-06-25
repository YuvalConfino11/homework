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
        private float m_DashDistance = 10f;
        [SerializeField]
        public float m_DashTime = 1f;
        private readonly AbilityStats r_AbilityStats;
        public bool isUnlocked;
        
        public Dash()
        {
            r_AbilityStats = new AbilityStats(false,true,m_CooldownTime);
        }

        public AbilityStats GetAbilityStats()
        {
            return r_AbilityStats;
        }

        public float DashDistance
        {
            get => m_DashDistance;
            set => m_DashDistance = value;
        }

        public float DashTime
        {
            get => m_DashTime;
            set => m_DashTime = value;
        }
    }
}