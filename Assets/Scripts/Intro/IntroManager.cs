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
        private TextMeshProUGUI textComp;

        [SerializeField]
        private SequenceScriptable sequences;
        [SerializeField]
        private VideoPlayer player;
        [SerializeField]
        Image blackMask;

        private int m_TextIndex = 0;
        private int m_TimeForText = 6;
        private float m_Timer;
        // Start is called before the first frame update
        void Start()
        {
            ShowNextText();
            player.prepareCompleted += Player_prepareCompleted;
            
        }

        private void Player_prepareCompleted(VideoPlayer source)
        {
            Debug.Log("Video is ready!");
            blackMask.DOFade(0, 2f);

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
            if(m_TextIndex == 5)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
