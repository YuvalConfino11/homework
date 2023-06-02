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
}
