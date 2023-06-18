using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnockbackScript : MonoBehaviour
{
    [SerializeField]
    private float m_KnockbackPower = 100;
    [SerializeField]
    private float m_KnockbackDuration = 1;
    [SerializeField]
    private Player m_player;
    

    private void Awake()
    {
        if (m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           // AudioManager.Instance.PlaySFX("")
            StartCoroutine(m_player.Knockback(m_KnockbackDuration , m_KnockbackPower , transform));
            m_player.GetHurtFeedback(0.2f);
            StartCoroutine(m_player.TimeItsRed(0.2f , 0.2f));


        }
    }
   
   

}
