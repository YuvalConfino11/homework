using System;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimations : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_ObjectiveLayerMask;
    [SerializeField]
    private GameObject m_SkillScreen;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_radius = 10f;
    private bool m_IsScreenOpen = false;
    private PauseControl m_PauseController;

    private void Awake()
    {
        m_PauseController = FindObjectOfType<PauseControl>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && m_IsScreenOpen == false)
        {
            Collider2D objectivesInRadius = Physics2D.OverlapCircle(transform.position, m_radius,m_ObjectiveLayerMask);
            if (objectivesInRadius != null)
            {
                AudioManager.Instance.PlaySFX("Angel");
                m_Animator.Play("Objective_Dissipate");
                StartCoroutine(ShowSkillScreen());
            }
        }
        if (m_IsScreenOpen == true && Input.GetKeyDown(KeyCode.Z))
        {
            m_SkillScreen.SetActive(false);
            m_IsScreenOpen = false;
            Time.timeScale = 1f;
            m_PauseController.IsGamePaused = false;
            this.gameObject.SetActive(false);
        }
    }
    
    private IEnumerator ShowSkillScreen()
    {
        yield return new WaitForSeconds(0.9f);
        m_SkillScreen.SetActive(true);
        Time.timeScale = 0f;
        m_PauseController.IsGamePaused = true;
        m_IsScreenOpen = true;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,m_radius);
    }
}
