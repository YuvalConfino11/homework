using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class Dash : Ability
    {
        private const short k_AvailabilityLevel = 2;
        private const float k_CooldownTime = 2f;
        
        public Dash()
        {
            this.SetAvailabilityLevel(k_AvailabilityLevel);
            this.SetIsAvailable(true);
        }

        public void RunAbility(float i_movingDirection, float i_dashSpeed, Rigidbody2D i_Rigidbody2D)
        {
            SetIsAvailable(false);
            i_Rigidbody2D.AddForce(new Vector2(i_movingDirection * i_dashSpeed, 0), ForceMode2D.Impulse);
            abilityCooldownTimer();

        }

        private void abilityCooldownTimer()
        {
            new WaitForSeconds(k_CooldownTime);
            SetIsAvailable(true);
            Debug.Log(GetIsAvailable());
        }
        
    }
}