namespace Abilities
{
    public class Ability
    {
        private bool m_isUnlocked = false;
        private bool m_isAvailableToUse = false;
        private short m_availabilityLevel;
        
        public bool getIsUnlocked()
        {
            return m_isUnlocked;
        }

        public void setIsUnlocked(bool i_isUnlocked)
        {
            m_isUnlocked = i_isUnlocked;
        }
        
        public bool getIsAvailable()
        {
            return m_isAvailableToUse;
        }

        public void setIsAvailable(bool i_isAvailable)
        {
            m_isAvailableToUse = i_isAvailable;
        }

        protected void setAvailabilityLevel(short i_AvailabilityLevel)
        {
            m_availabilityLevel = i_AvailabilityLevel;
        }
        
        public short getAvailabilityLevel()
        {
            return m_availabilityLevel;
        }
        
    }
    
    
}