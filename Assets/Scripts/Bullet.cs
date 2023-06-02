using Mobs;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject m_Player;
    [SerializeField]
    private float m_Range = 10f;
    [SerializeField] 
    private float m_Damage = 30f;
    
    private bool m_IsmPlayerNotNull;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if (m_Player != null)
        {
            if (Mathf.Abs(transform.position.x - m_Player.transform.position.x) > m_Range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D i_Collision)
    {
        if (i_Collision.gameObject.CompareTag("Mob"))
        {
            i_Collision.gameObject.GetComponent<MobStats>().GetHit(m_Damage);
            Destroy(this.gameObject);
        }
    }
   
}
