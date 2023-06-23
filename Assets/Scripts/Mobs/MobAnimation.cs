using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    public void PlayMobAnimation(float i_MobHorizontalVelocity)
    {
        m_Animator.SetFloat("MobMoveSpeed", Mathf.Abs(i_MobHorizontalVelocity));
    }

    public void PlayMobAttackAnimation()
    {
        m_Animator.SetBool("IsMobAttacking", true);
    }
    
    public void StopMobAttackAnimation()
    {
        m_Animator.SetBool("IsMobAttacking", false);
    }
}
