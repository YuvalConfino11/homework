using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour
{
    [SerializeField]
    private Slider m_Slider;
    [SerializeField]
    private Player m_Player;

    public void SetMana(float i_Mana)
    {
        m_Slider.value = i_Mana; 
    }

    public void SetMaxMana(float i_Mana)
    {
        m_Slider.maxValue = i_Mana;
        m_Slider.value = i_Mana;
    }
  
}
