using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    public void PlayPlayerAnimation(float i_PlayerHorizontalVelocity, float i_PlayerVerticalVelocity,
        bool i_IsGrounded)
    {
        m_Animator.SetFloat("HorizontalVelocity", Mathf.Abs(i_PlayerHorizontalVelocity));
        m_Animator.SetFloat("VerticalVelocity", i_PlayerVerticalVelocity);
        m_Animator.SetBool("IsGrounded", i_IsGrounded);
    }

    public void JumpAnimation()
    {
        m_Animator.SetTrigger("Jump");
    }
}