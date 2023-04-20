using UnityEngine;

namespace Abilities
{
    public class AbilityStats
    {
        private bool m_IsUnlocked;
        private bool m_IsAvailableToUse;
        private short m_AvailabilityLevel;
        private float m_CooldownTime;

        public AbilityStats(short i_AvailabilityLevel, bool i_IsUnlocked = false, bool i_IsAvailableToUse = true,
            float i_CooldownTime = 0)
        {
            m_IsUnlocked = i_IsUnlocked;
            m_IsAvailableToUse = i_IsAvailableToUse;
            m_AvailabilityLevel = i_AvailabilityLevel;
            m_CooldownTime = i_CooldownTime;


        }
        
        public bool GetIsUnlocked()
        {
            return m_IsUnlocked;
        }

        public void SetIsUnlocked(bool i_isUnlocked)
        {
            m_IsUnlocked = i_isUnlocked;
        }
        
        public bool GetIsAvailable()
        {
            return m_IsAvailableToUse;
        }

        public void SetIsAvailable(bool i_isAvailable)
        {
            m_IsAvailableToUse = i_isAvailable;
        }

        public void SetAvailabilityLevel(short i_AvailabilityLevel)
        {
            m_AvailabilityLevel = i_AvailabilityLevel;
        }
        
        public short GetAvailabilityLevel()
        {
            return m_AvailabilityLevel;
        }
        
        public void SetCooldownTime(float i_CooldownTime)
        {
            m_CooldownTime = i_CooldownTime;
        }
        
        public float GetCooldownTime()
        {
            return m_CooldownTime;
        }
        
    }
    
    
}