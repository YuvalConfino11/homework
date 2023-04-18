using UnityEngine;

namespace Abilities
{
    public class DoubleJump : Ability
    {
        private const short m_availabilityLevel = 2;
        
        public DoubleJump()
        {
            this.setAvailabilityLevel(m_availabilityLevel);
        }

        public void runAbility(float i_jumpHeight,Rigidbody2D i_rigidBody)
        {
            float jumpForce = Mathf.Sqrt( -2 * i_jumpHeight * (Physics2D.gravity.y * i_rigidBody.gravityScale));
            i_rigidBody.velocity = Vector2.up * jumpForce;
            // i_rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            setIsAvailable(false);
        }
        
    }

}