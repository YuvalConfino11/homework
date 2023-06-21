using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_RespawnPoint;
    [SerializeField]
    private Player m_Player;
    [SerializeField]
    private ImaginaryFriendAi m_ImaginaryFriend;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onCollisionHandler(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        onCollisionHandler(collision.collider);
    }

    private void onCollisionHandler(Collider2D i_Collider)
    {
        if (i_Collider.gameObject.CompareTag("Player")) 
        {
            m_Player.transform.position = m_RespawnPoint.transform.position;
            m_ImaginaryFriend.transform.position = m_RespawnPoint.transform.position;
        }
    }
}
