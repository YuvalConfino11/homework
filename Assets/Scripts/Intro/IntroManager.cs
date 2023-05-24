using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Sequences {

    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textComp;

        [SerializeField]
        private SequenceScriptable sequences;

        private int m_TextIndex = 0;
        private int m_TimeForText = 3;
        private float m_Timer;
        // Start is called before the first frame update
        void Start()
        {
            ShowNextText();
            
        }

        // Update is called once per frame
        void Update()
        {
            if((m_Timer += Time.deltaTime) >= m_TimeForText)
            {
                ShowNextText();
                m_Timer = 0;
            }
        }

        private void ShowNextText()
        {
            textComp.text = sequences.introSequences[m_TextIndex].m_MainText;
            m_TextIndex++;
            if(m_TextIndex == 3)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
