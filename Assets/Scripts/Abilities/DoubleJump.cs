using UnityEngine;

namespace Abilities
{
    public class DoubleJump : Ability
    {
        private const short k_AvailabilityLevel = 2;
        
        public DoubleJump()
        {
            this.SetAvailabilityLevel(k_AvailabilityLevel);
        }

        public void RunAbility(float i_jumpHeight,Rigidbody2D i_rigidBody)
        {
            float jumpForce = Mathf.Sqrt( -2 * i_jumpHeight * (Physics2D.gravity.y * i_rigidBody.gravityScale));
            i_rigidBody.velocity = Vector2.up * jumpForce;
            // i_rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            SetIsAvailable(false);
        }
        
    }

}