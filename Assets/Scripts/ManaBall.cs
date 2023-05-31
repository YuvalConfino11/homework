using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBall : MonoBehaviour
{
    [SerializeField]
    private float m_ManaPoints = 10f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().SetMana(m_ManaPoints);
            Destroy(gameObject);
        }
    }
}
