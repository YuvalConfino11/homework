using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;

namespace Sequences {

    public class IntroManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_TextComp;

        [SerializeField]
        private SequenceScriptable m_Sequences;
        [SerializeField]
        private VideoPlayer m_Player;
        [SerializeField]
        Image m_BlackMask;

        private int m_TextIndex = 0;
        private int m_TimeForText = 4;
        private float m_Timer;
        // Start is called before the first frame update
        void Start()
        {
            showNextText();
            m_Player.prepareCompleted += Player_prepareCompleted;
            
        }

        private void Player_prepareCompleted(VideoPlayer i_Source)
        {
            Debug.Log("Video is ready!");
            m_BlackMask.DOFade(0, 2f);

        }

        // Update is called once per frame
        void Update()
        {
            if((m_Timer += Time.deltaTime) >= m_TimeForText)
            {
                showNextText();
                m_Timer = 0;
            }
        }

        private void showNextText()
        {
            m_TextComp.text = m_Sequences.m_IntroSequences[m_TextIndex].m_MainText;
            m_TextIndex++;
            if(m_TextIndex == m_Sequences.m_IntroSequences.Length)
            {
                SceneManager.LoadScene("Tutorial");
            }
        }
    }
}
