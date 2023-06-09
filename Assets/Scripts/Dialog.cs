using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    [SerializeField]
    private GameObject m_DialogPanel;
    [SerializeField]
    public TextMeshProUGUI  m_DialogText;
    [SerializeField]
    private string[] m_Dialog;
    [SerializeField]
    private float m_DialogSpeed;
    [SerializeField]
    private bool m_DialogIsNear;
    [SerializeField]
    private bool m_OpenedDialog;
    [SerializeField]
    private int m_index;


    // Update is called once per frame
    void Update()
    {
        if (m_DialogIsNear && !m_OpenedDialog)
        {
            if (!m_DialogPanel.activeInHierarchy)
            {
                m_DialogPanel.SetActive(true);
                //Time.timeScale = 0;
                StartCoroutine(Typing());
            }
            else
            {
                m_OpenedDialog = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_DialogIsNear = true;
           
        }
    }

    private IEnumerator Typing()
    {
        foreach(char letter in m_Dialog[m_index].ToCharArray())
        {
            m_DialogText.text += letter;
            yield return new WaitForSeconds(m_DialogSpeed);
        }
    }

    public void NextLine()
    {
        Debug.Log(m_index);
        if(m_index < m_Dialog.Length - 1)
        {
            m_index++;
            m_DialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            Debug.Log("false");
           
            m_DialogPanel.SetActive(false);
            m_DialogText.text = "";
            m_index = 0;

            //Time.timeScale = 1;
        }
    }
}
