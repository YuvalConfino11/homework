using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScenesChanges : MonoBehaviour
{
    [SerializeField]
    Image m_BlackMask;
    [SerializeField]
    private float m_TimeBetweenScenes = 2f;
    [SerializeField]
    private float m_TimeForMask = 2f;
    [SerializeField]
    private GameObject m_QuitMenu;
    [SerializeField]
    private int m_QuitMenuCounter;

    private Color m_color;

    // Start is called before the first frame update
    void Start()
    {
        m_QuitMenu.SetActive(false);
        m_QuitMenuCounter = 0;
        ChangeScene(m_TimeBetweenScenes);
        StartCoroutine(DisableMask(m_TimeForMask));
        m_color = m_BlackMask.color;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_QuitMenuCounter == 0)
            {
                //change the alpha of black mask.
                m_color.a = 0.5f;  // change it between 0 and 1 where 1 is solid black and 0 is transparent.
                m_BlackMask.color = m_color;
                //do m_BlackMask.enabled = true;
                m_BlackMask.enabled = true;
                m_QuitMenu.SetActive(true);
                m_QuitMenuCounter++;
                Time.timeScale = 0;
            }
            else
            {
              
                m_QuitMenu.SetActive(false);
                //do m_BlackMask.enabled = false;
                m_BlackMask.enabled = false;
                m_QuitMenuCounter = 0;
                Time.timeScale = 1;
            }
            
        }
    }

    // Update is called once per frame
    void ChangeScene(float i_Time)
    {
        m_BlackMask.DOFade(0, i_Time);
    }
    private IEnumerator DisableMask(float i_TimeForMask)
    {
        yield return new WaitForSeconds(i_TimeForMask);
        m_BlackMask.enabled = false;
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void Continue()
    {
        m_QuitMenu.SetActive(false);
        m_QuitMenuCounter = 0;
        Time.timeScale = 1;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
