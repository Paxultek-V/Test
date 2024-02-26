using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ModeSelector : MonoBehaviour
{
    [SerializeField] private GameModes m_gameMode;

    [SerializeField] private Button m_button = null;

    private void OnEnable()
    {
        m_button.onClick.AddListener(SelectGameMode);
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(SelectGameMode);
    }

    private void SelectGameMode()
    {
        GameActions.ChangeGameMode(m_gameMode);
    }
}
