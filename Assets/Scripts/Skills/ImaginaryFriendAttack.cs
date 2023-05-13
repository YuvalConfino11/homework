using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skills
{
    [CreateAssetMenu (menuName = "Scriptable Objects/Imaginary Friend Attack" , fileName = "ImaginaryFriendAttack")]

    public class ImaginaryFriendAttack : ScriptableObject
    {

        [SerializeField]
        private short k_AvailabilityLevel = 2;
        [SerializeField]
        private float m_AttackRadius;
        private readonly SkillsStats m_SkillsStats;



        public ImaginaryFriendAttack()
        {
            m_SkillsStats = new SkillsStats(k_AvailabilityLevel);
        }

        public SkillsStats GetSkillsStats()
        {
            return m_SkillsStats;
        }

        public float GetAttackRadius()
        {
            return m_AttackRadius;
        }


    }
}