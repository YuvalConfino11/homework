using System.Threading.Tasks;
using UnityEngine;

namespace Abilities
{
    public class Dash : Ability
    {
        private const short k_AvailabilityLevel = 2;
        private const float k_CooldownTime = 1f;
        
        public Dash()
        {
            this.SetAvailabilityLevel(k_AvailabilityLevel);
            this.SetIsAvailable(true);
            this.SetCooldownTime(k_CooldownTime);
        }

        public void RunAbility(float i_movingDirection, float i_dashSpeed, Rigidbody2D i_Rigidbody2D)
        {
            SetIsAvailable(false);
            i_Rigidbody2D.AddForce(new Vector2(i_movingDirection * i_dashSpeed, 0), ForceMode2D.Impulse);
        }
        
    }
}