using UnityEngine;

namespace Abilities
{
    public class Glide : Ability
    {
        private const short k_AvailabilityLevel = 2;
        
        public Glide()
        {
            this.SetAvailabilityLevel(k_AvailabilityLevel);
        }

        public void RunAbility(float i_graviryFactor, Rigidbody2D i_rigidBody)
        {
                i_rigidBody.gravityScale = i_graviryFactor;
        }
    }

}