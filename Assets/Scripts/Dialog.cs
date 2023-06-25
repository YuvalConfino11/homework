using System;
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
    [SerializeField]
    private bool isTyping;
    [SerializeField]
    private bool m_IsDialogEnded;
    private PauseControl m_PauseController;


    private void Awake()
    {
        m_PauseController = FindObjectOfType<PauseControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_DialogIsNear && !m_OpenedDialog && !m_IsDialogEnded)
        {
            if (!m_DialogPanel.activeInHierarchy)
            {
                m_DialogPanel.SetActive(true);
                Time.timeScale = 0;
                m_PauseController.IsGamePaused = true;
                StartCoroutine(Typing());
            }
            else
            {
                m_OpenedDialog = true;
            }
           
        }
        if (m_OpenedDialog)
        {
            if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
            {
                NextLine();
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
        isTyping = true;
        foreach(char letter in m_Dialog[m_index].ToCharArray())
        {
            m_DialogText.text += letter;
            yield return new WaitForSecondsRealtime(m_DialogSpeed);
        }
        isTyping = false;
    }

    public void NextLine()
    {
        if(m_index < m_Dialog.Length - 1)
        {
            m_index++;
            m_DialogText.text = "";
            StartCoroutine(Typing());
        }
        else
        {  
            
            m_DialogText.text = "";
            m_index = 0;
            m_DialogPanel.SetActive(false);
            m_IsDialogEnded = true;
            Time.timeScale = 1;
            m_PauseController.IsGamePaused = false;
        }
    }
    
    public bool IsDialogEnded
    {
        get => m_IsDialogEnded;
        set => m_IsDialogEnded = value;
    }
}
