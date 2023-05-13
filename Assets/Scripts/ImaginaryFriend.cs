using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;

public class ImaginaryFriend : MonoBehaviour
{ 
    [SerializeField] 
    private float m_UpAndDownMovementSpeed = 0.2f;
    [SerializeField]
    private ImaginaryFriendAttack m_ImaginaryFriendAttack;

    private Collider2D m_MobInAttackRadius;
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

    private void attack()
    {
        float attackRadius = m_ImaginaryFriendAttack.GetAttackRadius();
        m_MobInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius);
        float distance = Vector2.Distance(transform.position, m_MobInAttackRadius.transform.position);
    }
}
