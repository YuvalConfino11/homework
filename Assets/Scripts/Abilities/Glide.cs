using UnityEngine;

namespace Abilities
{
    public class Glide : Ability
    {
        private const short m_availabilityLevel = 2;
        
        public Glide()
        {
            this.setAvailabilityLevel(m_availabilityLevel);
        }

        public void runAbility(float i_graviryFactor, Rigidbody2D i_rigidBody)
        {
                i_rigidBody.gravityScale = i_graviryFactor;
        }
    }

}