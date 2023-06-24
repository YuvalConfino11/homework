using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCrushers : MonoBehaviour
{
    [SerializeField]
    private GameObject crusher;
    [SerializeField]
    public bool m_ActivateCrushers;
    [SerializeField]
    private Transform m_StartPos;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_ActivateCrushers = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_ActivateCrushers = false;
            crusher.transform.position = m_StartPos.position;
        }
    }
}
