using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{

    [SerializeField]
    private float m_upAndDownMovementSpeed = 2f;
    [SerializeField]
    private Animator m_KeyAnimator;
    [SerializeField] private float m_radius = 10f;
    [SerializeField] private LayerMask m_ObjectiveLayerMask;


    private int m_upAndDownMovementDirection = 1;
    // Update is called once per frame
    
    void Update()
    {
        float newYPosition = m_upAndDownMovementSpeed * m_upAndDownMovementDirection * Time.deltaTime;
        if (transform.position.y - transform.parent.position.y >= 3)
        {
            m_upAndDownMovementDirection = -1;
        }
        else if (transform.position.y - transform.parent.position.y <= 0.1)
        {
            m_upAndDownMovementDirection = 1;
        }
        transform.Translate(0, newYPosition, 0);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Collider2D objectivesInRadius = Physics2D.OverlapCircle(transform.position, m_radius, m_ObjectiveLayerMask);
            if (objectivesInRadius != null)
            {
                if (objectivesInRadius.name == "Player")
                {
                    AudioManager.Instance.PlaySFX("Angel");
                    m_KeyAnimator.Play("Key_Dissipate");
                    StartCoroutine(SetActiveToFalse(this.gameObject, 0.8f));
                }
            }
        }
    }
    public IEnumerator SetActiveToFalse(GameObject i_GameObject, float i_TimeToFalse)
    {
        yield return new WaitForSeconds(i_TimeToFalse);
        i_GameObject.SetActive(false);
    }
}

