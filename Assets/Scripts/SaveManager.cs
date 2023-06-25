using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Abilities;
using Mobs;
using Skills;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private GateScript[] m_Gates;
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
    [SerializeField]
    private GameObject m_DashObjective;
    [SerializeField]
    private GameObject m_DoubleJumpObjective;
    [SerializeField]
    private GameObject m_GlideObjective;
    [SerializeField]
    private GameObject m_HealObjective;
    [SerializeField]
    private GameObject m_ExplosionObjective;
    [SerializeField]
    private Dialog[] m_Dialogs;
    private bool m_DashAvailability;
    private bool m_DoubleJumpAvailability;
    private bool m_GlideAvailability;
    private bool m_EnergyExplosionAvailability;
    private bool m_HealAvailability;
    private bool[] m_IsGatesOpened;
    private bool[] m_IsDialogsEnded;
    private float m_GateXPos;
    private float m_GateYPos;

    // Start is called before the first frame update
    void Start()
    {
        m_DashAvailability = intToBool( PlayerPrefs.GetInt("DashAvailability"));
        m_DoubleJumpAvailability = intToBool(PlayerPrefs.GetInt("DoubleJumpAvailability"));
        m_GlideAvailability = intToBool(PlayerPrefs.GetInt("GlideAvailability"));
        m_EnergyExplosionAvailability = intToBool(PlayerPrefs.GetInt("EnergyExplosionAvailability"));
        m_HealAvailability = intToBool(PlayerPrefs.GetInt("HealAvailability" ));
        m_IsGatesOpened = new bool[m_Gates.Length];
        m_IsDialogsEnded = new bool[m_Dialogs.Length];
        for (int i = 0; i < m_Gates.Length; i++)
        {
            m_IsGatesOpened[i] = intToBool(PlayerPrefs.GetInt("GateOpened"+ (i + 1)));
        }
        for (int i = 0; i < m_Dialogs.Length; i++)
        {
            m_IsDialogsEnded[i] = intToBool(PlayerPrefs.GetInt("Dialog"+ (i + 1)));
        }
        PlayerPrefs.Save();
        
        SetSavedAbilityStats(m_Dash.GetAbilityStats(), m_DashAvailability);
        SetSavedAbilityStats(m_DoubleJump.GetAbilityStats(), m_DoubleJumpAvailability);
        SetSavedAbilityStats(m_Glide.GetAbilityStats(), m_GlideAvailability);

        SetSavedSkillStats(m_EnergyExplosion.GetSkillsStats(), m_EnergyExplosionAvailability);
        SetSavedSkillStats(m_Heal.GetSkillsStats(), m_HealAvailability);
        PlayerPrefs.Save();

      
        if (m_DashAvailability)
        {
            DestroyObjective(m_DashObjective);
        }
        if (m_DoubleJumpAvailability)
        {
            DestroyObjective(m_DoubleJumpObjective);
        }
        if (m_GlideAvailability)
        {
            DestroyObjective(m_GlideObjective);
        }
        if (m_EnergyExplosionAvailability)
        {
            DestroyObjective(m_ExplosionObjective);
        }
        if (m_HealAvailability)
        {
            DestroyObjective(m_HealObjective);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetInt("DashAvailability", boolToInt(m_Dash.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("GlideAvailability", boolToInt(m_Glide.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("DoubleJumpAvailability", boolToInt(m_DoubleJump.GetAbilityStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("HealAvailability", boolToInt(m_Heal.GetSkillsStats().GetIsUnlocked()));
        PlayerPrefs.SetInt("EnergyExplosionAvailability", boolToInt(m_EnergyExplosion.GetSkillsStats().GetIsUnlocked()));
        for (int i = 0; i < m_Gates.Length; i++)
        {
            PlayerPrefs.SetInt("GateOpened"+ (i + 1),boolToInt(m_Gates[i].HasGateOpened()));
            if (m_IsGatesOpened[i])
            {
                m_Gates[i].MoveGate();
            }
        }
        for (int i = 0; i < m_Dialogs.Length; i++)
        {
            PlayerPrefs.SetInt("Dialog"+ (i + 1),boolToInt(m_Dialogs[i].IsDialogEnded));
            if (m_IsDialogsEnded[i])
            {
                m_Dialogs[i].IsDialogEnded = true;
            }
        }
        PlayerPrefs.Save();
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

    private void DestroyObjective(GameObject objective)
    {
        objective.gameObject.SetActive(false);
    }
   
}
