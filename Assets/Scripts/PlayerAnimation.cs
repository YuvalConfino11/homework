using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    public void PlayPlayerAnimation(float i_PlayerHorizontalVelocity, float i_PlayerVerticalVelocity,
        bool i_IsGrounded)
    {
        m_animator.SetFloat("HorizontalVelocity", Mathf.Abs(i_PlayerHorizontalVelocity));
        m_animator.SetFloat("VerticalVelocity", i_PlayerVerticalVelocity);
        m_animator.SetBool("IsGrounded", i_IsGrounded);
    }

    public void JumpAnimation()
    {
        m_animator.SetTrigger("Jump");
    }
    
    public void DashAnimation()
    {
        m_animator.SetTrigger("Dash");
    }
    
    public void GlideAnimation()
    {
        m_animator.SetTrigger("Glide");
    }
    
    public void EndGlideAnimation()
    {
        m_animator.SetTrigger("EndGlide");
    }
}