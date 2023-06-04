using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private Transform m_OpenGatePos;
    [SerializeField]
    private float  m_GateMoveSpeed = 3;
    [SerializeField]
    private bool m_PlayerTouchedGate;

    private void Update()
    {
        if (m_PlayerTouchedGate)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_OpenGatePos.position, m_GateMoveSpeed * Time.deltaTime);
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (m_player.PlayerGotKey())
            {
                Debug.Log("Gate Open");
                m_PlayerTouchedGate = true;
               
            }
           
        }
    }
}
