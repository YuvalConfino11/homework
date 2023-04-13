using UnityEngine;

namespace Skills
{
    public static class DoubleJump
    {
        private const short k_availableFromLevel = 2;
        private const short k_coolDownTime = 7;
        
        public static short GetCooldownTime()
        {
            return k_coolDownTime;
        }

        public static short GetAvailableFromLevel()
        {
            return k_availableFromLevel;
        }
    }
    
    
}