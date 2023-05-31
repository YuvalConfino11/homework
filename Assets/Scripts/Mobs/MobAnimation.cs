using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    public void PlayMobAnimation(float i_MobHorizontalVelocity)
    {
        m_animator.SetFloat("MobMoveSpeed", Mathf.Abs(i_MobHorizontalVelocity));
    }
}
