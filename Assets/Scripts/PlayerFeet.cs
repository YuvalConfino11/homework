using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerFeet : MonoBehaviour
    {
        [SerializeField] 
        private Player m_player;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform"))
            {
                m_player.setCanJump(true);
                if (m_player.getCanDoubleJump())
                {
                    m_player.resetDoubleJumpCount();
                }
            }
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform"))
            {
                m_player.setCanJump(true);
                if (m_player.getCanDoubleJump())
                {
                    m_player.resetDoubleJumpCount();
                }
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PushPlatform"))
            {
                m_player.setCanJump(false);
            }
        }
    }
}