using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject m_player;
    [SerializeField]
    private float m_range = 10f;

    private bool ismPlayerNotNull;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        if (m_player != null)
        {
            if (Mathf.Abs(transform.position.x - m_player.transform.position.x) > m_range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            Destroy(this.gameObject);
    }
   
}
