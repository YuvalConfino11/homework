using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_RespawnPoint;
    [SerializeField]
    private Player m_Player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            m_Player.transform.position = m_RespawnPoint.transform.position;        
        }
    }
}
