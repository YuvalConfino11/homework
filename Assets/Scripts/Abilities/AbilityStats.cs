using UnityEngine;

namespace Abilities
{
    public class AbilityStats
    {
        private bool m_IsUnlocked;
        private bool m_IsAvailableToUse;
        private short m_AvailabilityLevel;
        private float m_CooldownTime;

        public AbilityStats(bool i_IsUnlocked = false, bool i_IsAvailableToUse = true, float i_CooldownTime = 0)
        {
            m_IsUnlocked = i_IsUnlocked;
            m_IsAvailableToUse = i_IsAvailableToUse;
            m_CooldownTime = i_CooldownTime;
        }
        
        public bool GetIsUnlocked()
        {
            return m_IsUnlocked;
        }

        public void SetIsUnlocked(bool i_IsUnlocked)
        {
            m_IsUnlocked = i_IsUnlocked;
        }
        
        public bool GetIsAvailable()
        {
            return m_IsAvailableToUse;
        }

        public void SetIsAvailable(bool i_IsAvailable)
        {
            m_IsAvailableToUse = i_IsAvailable;
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