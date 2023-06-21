using System.Collections;
using System.Collections.Generic;
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
    private Player m_player;

    // Start is called before the first frame update
    void Start()
    {
        ChangeScene(m_TimeBetweenScenes);
        StartCoroutine(DisableMask(m_TimeForMask));
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
}
