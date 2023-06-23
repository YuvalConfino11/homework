using System.Collections;
using Mobs;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject m_Player;
    [SerializeField]
    private float m_Range = 100f;
    [SerializeField] 
    private float m_Damage = 30f;
    [SerializeField] 
    private float m_Speed = 25f;
    
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
            this.gameObject.GetComponent<Animator>().SetBool("Hit",true);
            StartCoroutine(AnimationCooldown(0.6f));
            this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
            m_Player.GetComponent<Player>().SetMana(2);
        }
    }
    
    public IEnumerator AnimationCooldown(float i_TimeToWait)
    {
        yield return new WaitForSeconds(i_TimeToWait);
        Destroy(this.gameObject);
    }

    public float GetSpeed()
    {
        return m_Speed;
    }

}
