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
        //Debug.Log("is grounded: " + i_IsGrounded + " x is: " + i_PlayerHorizontalVelocity + " y is: " + i_PlayerVerticalVelocity);
    }

    public void JumpAnimation()
    {
        m_animator.SetTrigger("Jump");
        Debug.Log("Did it happen?");
    }
}