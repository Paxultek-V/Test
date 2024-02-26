using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ModuleController : MonoBehaviour
{
    [SerializeField]
    private List<ModuleBase> m_playerModuleList;

    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += EnableMovement;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= EnableMovement;
    }

    private void EnableMovement()
    {
        StartCoroutine(InitializePlayerModules());
    }
    
    private IEnumerator InitializePlayerModules()
    {
        for (int i = 0; i < m_playerModuleList.Count; i++)
        {
            yield return m_playerModuleList[i].IE_Initialize();
        }
    }
}
