using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImaginaryFriend : MonoBehaviour
{ 
    [SerializeField] 
    private float m_upAndDownMovementSpeed = 0.2f;
    
    private int m_upAndDownMovementDirection = 1;

    void Update()
    {
        float newYPosition = m_upAndDownMovementSpeed * m_upAndDownMovementDirection * Time.deltaTime;
        if (transform.position.y - transform.parent.position.y >= 0.6)
        {
            m_upAndDownMovementDirection = -1;
        }
        else if (transform.position.y - transform.parent.position.y <= 0.4)
        {
            m_upAndDownMovementDirection = 1;
        }
        transform.Translate(0, newYPosition,0);
    }
}
