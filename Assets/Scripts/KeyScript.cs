using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

    [SerializeField]
    private float m_upAndDownMovementSpeed = 0.2f;


    private int m_upAndDownMovementDirection = 1;
    // Update is called once per frame
    
    void Update()
    {
        float newYPosition = m_upAndDownMovementSpeed * m_upAndDownMovementDirection * Time.deltaTime;
        if (transform.position.y - transform.parent.position.y >= 0.9)
        {
            m_upAndDownMovementDirection = -1;
        }
        else if (transform.position.y - transform.parent.position.y <= 0.1)
        {
            m_upAndDownMovementDirection = 1;
        }
        transform.Translate(0, newYPosition, 0);
    }
}

