using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            StartCoroutine(m_player.Knockback(m_KnockbackDuration , m_KnockbackPower , transform));
        }
    }

}
