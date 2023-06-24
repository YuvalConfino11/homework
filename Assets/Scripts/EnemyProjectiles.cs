using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;
    [SerializeField]
    private float m_ProjForce;
    [SerializeField]
    private float m_EnemyProjDamage;

    private Rigidbody2D m_Rb;
    // Start is called before the first frame update
    void Start()
    {
        if (m_Player == null)
        {
            m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        m_Rb = GetComponent<Rigidbody2D>();

        Vector3 m_Dir = m_Player.transform.position - transform.position;
        float angle = Mathf.Atan2(m_Dir.y, m_Dir.x) * Mathf.Rad2Deg;
        m_Rb.velocity = new Vector2(m_Dir.x , m_Dir.y).normalized * m_ProjForce;
        transform.Rotate(0,0,angle);
       
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().getHit(m_EnemyProjDamage);
            Destroy(gameObject);
        }
    }
}
