using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImaginaryFriend : MonoBehaviour
{ 
    [SerializeField] 
    private float m_UpAndDownMovementSpeed = 0.2f;
    
    private short m_UpAndDownMovementDirection = 1;

    void Update()
    {
        float newYPosition = m_UpAndDownMovementSpeed * m_UpAndDownMovementDirection * Time.deltaTime;
        if (transform.position.y - transform.parent.position.y >= 0.6)
        {
            m_UpAndDownMovementDirection = -1;
        }
        else if (transform.position.y - transform.parent.position.y <= 0.4)
        {
            m_UpAndDownMovementDirection = 1;
        }
        transform.Translate(0, newYPosition,0);
    }
}
