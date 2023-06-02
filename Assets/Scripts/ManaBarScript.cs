using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider;
    [SerializeField]
    private Player m_player;

    public void SetMana(float mana)
    {
        m_slider.value = mana; 
    }

    public void SetMaxMana(float mana)
    {
        m_slider.maxValue = mana;
        m_slider.value = mana;
    }
  
}
