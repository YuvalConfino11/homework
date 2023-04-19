namespace Abilities
{
    public class Ability
    {
        private bool m_IsUnlocked = false;
        private bool m_IsAvailableToUse = false;
        private short m_AvailabilityLevel;
        private float m_CooldownTime = 0f;
        
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

        protected void SetAvailabilityLevel(short i_AvailabilityLevel)
        {
            m_AvailabilityLevel = i_AvailabilityLevel;
        }
        
        public short GetAvailabilityLevel()
        {
            return m_AvailabilityLevel;
        }
        
        protected void SetCooldownTime(float i_CooldownTime)
        {
            m_CooldownTime = i_CooldownTime;
        }
        
        public float GetCooldownTime()
        {
            return m_CooldownTime;
        }
        
    }
    
    
}