using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using Mobs;
using Skills;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private GateScript m_FirstGate;
    [SerializeField]
    private Dash m_Dash;
    [SerializeField]
    private DoubleJump m_DoubleJump;
    [SerializeField]
    private Glide m_Glide;
    [SerializeField]
    private EnergyExplosion m_EnergyExplosion;
    [SerializeField]
    private Heal m_Heal;
    [SerializeField]
    private int m_TimesStartedScene;

    private bool m_DashAvailability;
    private bool m_DoubleJumpAvailability;
    private bool m_GlideAvailability;
    private bool m_EnergyExplosionAvailability;
    private bool m_HealAvailability;
    private bool m_GateOpened;
    private float m_GateXPos;
    private float m_GateYPos;

    // Start is called before the first frame update
    void Start()
    {
        m_DashAvailability = intToBool( PlayerPrefs.GetInt("DashAvailability", 0));
        m_DoubleJumpAvailability = intToBool(PlayerPrefs.GetInt("DoubleJumpAvailability" , 0));
        m_GlideAvailability = intToBool(PlayerPrefs.GetInt("GlideAvailability" , 0));
        m_EnergyExplosionAvailability = intToBool(PlayerPrefs.GetInt("EnergyExplosionAvailability", 0));
        m_HealAvailability = intToBool(PlayerPrefs.GetInt("HealAvailability" , 0));
        m_GateOpened = intToBool(PlayerPrefs.GetInt("GateOpened"));

        Debug.Log(m_DashAvailability);

        SetSavedAbilityStats(m_Dash.GetAbilityStats(), m_DashAvailability);
        SetSavedAbilityStats(m_DoubleJump.GetAbilityStats(), m_DoubleJumpAvailability);
        SetSavedAbilityStats(m_Glide.GetAbilityStats(), m_GlideAvailability);

        SetSavedSkillStats(m_EnergyExplosion.GetSkillsStats(), m_EnergyExplosionAvailability);
        SetSavedSkillStats(m_Heal.GetSkillsStats(), m_HealAvailability);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("TimesStartedScene", m_TimesStartedScene);
        PlayerPrefs.SetInt("DashAvailability", boolToInt(m_Dash.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("GlideAvailability", boolToInt(m_Glide.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("DoubleJumpAvailability", boolToInt(m_DoubleJump.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("HealAvailability", boolToInt(m_Heal.GetSkillsStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("EnergyExplosionAvailability", boolToInt(m_EnergyExplosion.GetSkillsStats().GetIsUnlocked()));

        PlayerPrefs.SetInt("GateOpened",boolToInt(m_FirstGate.HasGateOpened()));
        PlayerPrefs.Save();
      


        if (m_GateOpened)
        {
          
            m_FirstGate.MoveGate();
        }



    }
    private int boolToInt(bool i_val)
    {
        if (i_val)
            return 1;
        else
            return 0;
    }

    private bool intToBool(int i_val)
    {
        if (i_val != 0)
            return true;
        else
            return false;
    }
    private void SetSavedAbilityStats(AbilityStats i_ability , bool i_availibility) 
    {
        i_ability.SetIsUnlocked(i_availibility);
        i_ability.SetIsAvailable(i_availibility);
    }
    private void SetSavedSkillStats(SkillsStats i_Skill, bool i_availibility)
    {
        i_Skill.SetIsUnlocked(i_availibility);
        i_Skill.SetIsAvailable(i_availibility);
    }
   
}
