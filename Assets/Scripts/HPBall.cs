using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBall : MonoBehaviour
{
    [SerializeField]
    private float m_HPPoints = 5f;
    private void OnCollisionEnter2D(Collision2D i_Collision)
    {
        if (i_Collision.gameObject.CompareTag("Player"))
        {
            i_Collision.gameObject.GetComponent<Player>().getHit(-m_HPPoints);
            Destroy(gameObject);
        }
    }
}
