using System;
using UnityEngine;
using TMPro;

public class _Tester : MonoBehaviour
{
    [SerializeField] private TMP_Text m_gameModeText = null;


    private void OnEnable()
    {
        GameActions.ChangeGameMode += ChangeGameMode;
    }

    private void OnDisable()
    {
        GameActions.ChangeGameMode -= ChangeGameMode;
    }


    private void ChangeGameMode(GameModes newMode)
    {
        m_gameModeText.text = newMode.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            GameActions.StartGameMode?.Invoke();
        if (Input.GetKeyDown(KeyCode.R))
            GameActions.EndGameMode?.Invoke();
        if (Input.GetKeyDown(KeyCode.T))
            GameActions.ReloadGameMode?.Invoke();

        if (Input.GetKeyDown(KeyCode.D))
            GameActions.ChangeGameMode?.Invoke(GameModes.ModeBoss);
        if (Input.GetKeyDown(KeyCode.F))
            GameActions.ChangeGameMode?.Invoke(GameModes.ModeRunner);
    }
}