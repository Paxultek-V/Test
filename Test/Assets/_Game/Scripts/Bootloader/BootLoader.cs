using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum GameModes
{
    ModeBoss,
    ModeRunner
}

public class BootLoader : SerializedMonoBehaviour
{
    public static BootLoader Instance;

    public GameModes m_gameMode;

    public Dictionary<GameModes, GameModeLoader> m_gameModeLoaders = new Dictionary<GameModes, GameModeLoader>();

    public Dictionary<Type, ManagerBase> m_managersDictionary = new Dictionary<Type, ManagerBase>();

    private bool IsBusy { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        // SAVE MONEY

        FlushActionEvents();

        // safety check in editor
        Instance = null;

        UnititializeGameModes();
    }

    private IEnumerator Start()
    {
        IsBusy = true;

        // LOAD SAVE example with money

        // SEND EVENT ON MONEY CHANGED
        //GameActions.OnCurrencyChanged(Enum_PlayerData.PlayerCurrency.GetData<int>());

        yield return IE_Initialize();

        yield return IE_PostInitialize();

        yield return IE_SetupActionEvents();

        yield return IE_Activate();

        yield return IE_ChangeGameMode(GameModes.ModeBoss);

        IsBusy = false;
        yield return null;
        yield return null;

        UIAccess.DisableAll(Enum_UI_Canvas.Canvas_Start);

        GameActions.AfterGameLoaded?.Invoke();
        yield return null;
    }

    private IEnumerator IE_Initialize()
    {
        yield return m_managersDictionary.IE_InitializeAllManagers();
    }

    private IEnumerator IE_PostInitialize()
    {
        yield return m_managersDictionary.IE_PostInitializeAllManagers();
    }

    private IEnumerator IE_Activate()
    {
        yield return m_managersDictionary.IE_ActivateAllManagers();
    }

    public IEnumerator IE_SwitchStatus(ModuleStatus moduleStatus)
    {
        foreach (KeyValuePair<Type, ManagerBase> managerBaseMono in m_managersDictionary)
        {
            yield return managerBaseMono.Value.IE_SwitchStatus(moduleStatus);
        }
    }

    private IEnumerator IE_SetupActionEvents()
    {
        IsBusy = true;
        GameActions.StartGameMode += StartGameMode;
        GameActions.SwitchModuleStatus += SwitchModuleStatus;
        GameActions.EndGameMode += EndGameMode;
        GameActions.ChangeGameMode += ChangeGameMode;
        GameActions.ReloadGameMode += ReloadGameMode;
        IsBusy = false;
        yield return null;
    }

    private void FlushActionEvents()
    {
        GameActions.StartGameMode -= StartGameMode;
        GameActions.SwitchModuleStatus -= SwitchModuleStatus;
        GameActions.EndGameMode -= EndGameMode;
        GameActions.ChangeGameMode -= ChangeGameMode;
        GameActions.ReloadGameMode -= ReloadGameMode;
    }


    private void StartGameMode()
    {
        if (IsBusy) return;
        StartCoroutine(IE_StartGameMode());
    }

    private void EndGameMode()
    {
        if (IsBusy) return;
        StartCoroutine(IE_EndGameMode());
    }

    private void ChangeGameMode(GameModes gameMode)
    {
        if (IsBusy) return;
        StartCoroutine(IE_ChangeGameMode(gameMode));
    }

    private void ReloadGameMode()
    {
        if (IsBusy) return;
        StartCoroutine(IE_ReloadGameMode());
    }


    private IEnumerator IE_ChangeGameMode(GameModes gameMode)
    {
        if (this.m_gameMode == gameMode && GetCurrentGameModeLoader().Status == ModuleStatus.PostInitialized)
        {
            yield break;
        }
        
        if (GetCurrentGameModeLoader().Status != ModuleStatus.PostInitialized)
        {
            Enum_UI_Canvas.Canvas_Loading.Show();
        }

        IsBusy = true;
        GameActions.onBeforeGameModeChanged?.Invoke(gameMode);
        yield return GetCurrentGameModeLoader().IE_Deactivate();
        yield return GetCurrentGameModeLoader().IE_Deinitialize();
        this.m_gameMode = gameMode;

        yield return GetCurrentGameModeLoader().IE_Initialize();
        yield return GetCurrentGameModeLoader().IE_PostInitialize();
        GameActions.onAfterGameModeChanged?.Invoke(gameMode);

        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }

        Enum_UI_Canvas.Canvas_Loading.Hide();

        IsBusy = false;
    }


    private IEnumerator IE_ReloadGameMode()
    {
        IsBusy = true;
        GameActions.onBeforeGameModeChanged?.Invoke(m_gameMode);

        Enum_UI_Canvas.Canvas_Loading.Show();

        yield return GetCurrentGameModeLoader().IE_Deactivate();
        yield return GetCurrentGameModeLoader().IE_Deinitialize();
        yield return GetCurrentGameModeLoader().IE_Initialize();
        yield return GetCurrentGameModeLoader().IE_PostInitialize();

        for (int i = 0; i < 2; i++)
        {
            yield return null;
        }

        Enum_UI_Canvas.Canvas_Loading.Hide();

        GameActions.onAfterGameModeChanged?.Invoke(m_gameMode);
        IsBusy = false;
    }

    private IEnumerator IE_StartGameMode()
    {
        IsBusy = true;
        GameActions.onBeforeGameModeStarted?.Invoke();
        yield return GetCurrentGameModeLoader().IE_Activate();
        GameActions.onAfterGameModeStarted?.Invoke();
        IsBusy = false;
    }

    private IEnumerator IE_ReactivateGameMode()
    {
        IsBusy = true;
        yield return GetCurrentGameModeLoader().IE_ReActivate();
        IsBusy = false;
    }

    private IEnumerator IE_EndGameMode()
    {
        IsBusy = true;
        GameActions.onBeforeGameModeEnded?.Invoke();

        yield return GetCurrentGameModeLoader().IE_Deactivate();

        OnGameModeEnd();

        yield return null;
        GameActions.onAfterGameModeEnded?.Invoke();
        IsBusy = false;
    }

    private void SwitchModuleStatus(ModuleStatus moduleStatus)
    {
        StartCoroutine(GetCurrentGameModeLoader().SwitchModuleStatus(moduleStatus));
    }

    private void OnGameModeEnd()
    {
        switch (m_gameMode)
        {
            case GameModes.ModeBoss:
                GameModeController_Boss gm = GetCurrentGameModeLoader() as GameModeController_Boss;
                
                if(gm.IsWin)
                    UIAccess.DisableAll(Enum_UI_Canvas.Canvas_WinScreen);
                else
                    UIAccess.DisableAll(Enum_UI_Canvas.Canvas_LoseScreen);
                
                break;
            case GameModes.ModeRunner:
                UIAccess.DisableAll(Enum_UI_Canvas.Canvas_LoseScreen);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public GameModeLoader GetCurrentGameModeLoader()
    {
        if (m_gameModeLoaders.TryGetValue(m_gameMode, out GameModeLoader gameModeLoader))
            return gameModeLoader;
        else
            return m_gameModeLoaders[GameModes.ModeBoss];
    }

    public T GetManager<T>() where T : ManagerBase
    {
        return (T)m_managersDictionary[typeof(T)];
    }


    private void UnititializeGameModes()
    {
        foreach (var gameMode in m_gameModeLoaders)
        {
            gameMode.Value.ForceSetStatus(ModuleStatus.Uninitialized);
        }
    }
    
}