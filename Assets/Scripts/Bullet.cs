using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    [SerializeField]
    private Player m_player;
    [SerializeField]
    private float m_range = 10f;

    void Update()
    {
        if(m_player != null)
        {
            if (Mathf.Abs(transform.position.x - m_player.GetPosition().x) > m_range)
            {
                Destroy(this.gameObject);
            }
        }
        

       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

    }
}
