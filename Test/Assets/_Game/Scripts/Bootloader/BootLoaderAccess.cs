using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BootLoaderAccess
{
    public static T GetManager<T>() where T : ManagerBase
    {
        return BootLoader.Instance.GetManager<T>();
    }

    private static UIManager uiManager;

    public static UIManager UiManager
    {
        get
        {
            if (uiManager == null)
            {
                uiManager = GetManager<UIManager>();
                return uiManager;
            }

            return uiManager;
        }
    }


    public static bool GetManager<T>(out T managerBaseMono) where T : ManagerBase
    {
        managerBaseMono = BootLoader.Instance.GetManager<T>();
        return managerBaseMono != null;
    }

    public static IEnumerator IE_InitializeAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_Initialize();
        }
    }

    public static IEnumerator IE_PostInitializeAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_PostInitialize();
        }
    }

    public static IEnumerator IE_ActivateAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_Activate();
        }
    }

    public static IEnumerator IE_DeactivateAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_Deactivate();
        }
    }

    public static IEnumerator IE_DeinitializeAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_Deinitialize();
        }
    }

    public static IEnumerator IE_PauseAllManagers(this Dictionary<Type, ManagerBase> managerBase)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in managerBase)
        {
            yield return managerBaseMono.Value.IE_Pause();
        }
    }

    public static IEnumerator IE_SwitchAllManagerStatus(this ModuleStatus status)
    {
        yield return BootLoader.Instance.IE_SwitchStatus(status);
    }

    public static GameModes CurrentGameMode => BootLoader.Instance.m_gameMode;
}