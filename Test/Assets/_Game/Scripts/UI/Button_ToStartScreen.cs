using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_ToStartScreen : MonoBehaviour
{
    [SerializeField] private Button m_button = null;

    private void Start()
    {
        m_button.onClick.AddListener(ToStartScreen);
    }

    private void ToStartScreen()
    {
        GameActions.ReloadGameMode?.Invoke();
    }
}
