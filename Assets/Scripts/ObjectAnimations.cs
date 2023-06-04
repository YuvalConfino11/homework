using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimations : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_ObjectiveLayerMask;

    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_radius = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider2D objectivesInRadius = Physics2D.OverlapCircle(transform.position, m_radius,m_ObjectiveLayerMask);
            if (objectivesInRadius.name == "Player")
            {
                m_Animator.Play("Objective_Dissipate");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,m_radius);
    }
}
