using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skills
{
    [CreateAssetMenu (menuName = "Scriptable Objects/Imaginary Friend Attack" , fileName = "ImaginaryFriendAttack")]

    public class ImaginaryFriendAttack : ScriptableObject
    {

        [SerializeField]
        private short m_AvailabilityLevel = 2;
        [SerializeField]
        private float m_AttackRadius = 50;
        [SerializeField]
        private float m_AttackDamage = 30;
        private readonly SkillsStats r_SkillsStats;



        public ImaginaryFriendAttack()
        {
            r_SkillsStats = new SkillsStats(true,true);
        }

        public SkillsStats GetSkillsStats()
        {
            return r_SkillsStats;
        }

        public float GetAttackRadius()
        {
            return m_AttackRadius;
        }

        public float getAttackDamage()
        {
            return m_AttackDamage;
        }
        
    }
}