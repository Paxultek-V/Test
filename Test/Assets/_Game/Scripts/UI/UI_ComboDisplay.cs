using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_ComboDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text m_comboText = null;

    [SerializeField] private Animator m_comboDisplayAnimator = null;

    [SerializeField] private Color m_baseComboColor = Color.white;
    
    [SerializeField] private Color m_maxComboColor = Color.white;
    
    private StringBuilder m_str;
    private int m_comboToDisplay;
    
    private void OnEnable()
    {
        Controller_BounceCombo.OnSendCombo += OnSendCombo;
        Controller_BounceCombo.OnSendComboProgression += OnSendComboProgression;
    }

    private void OnDisable()
    {
        Controller_BounceCombo.OnSendCombo -= OnSendCombo;
        Controller_BounceCombo.OnSendComboProgression -= OnSendComboProgression;
    }

    private void Start()
    {
        m_str = new StringBuilder();
        UpdateComboText(m_comboToDisplay);
    }


    private void OnSendComboProgression(float comboProgression)
    {
        m_comboText.color = Color.Lerp(m_baseComboColor, m_maxComboColor, comboProgression);
    }
    
    private void OnSendCombo(int combo)
    {
        m_comboToDisplay = combo;
        UpdateComboText(m_comboToDisplay);
    }

    private void UpdateComboText(int combo)
    {
        if (combo == 0)
        {
            m_comboText.text = "";
            return;
        }

        m_str.Clear();
        m_str.Append("Combo\nx");
        m_str.Append(m_comboToDisplay);
        m_comboText.text = m_str.ToString();
        
        m_comboDisplayAnimator.SetTrigger("Bounce");
    }
}
