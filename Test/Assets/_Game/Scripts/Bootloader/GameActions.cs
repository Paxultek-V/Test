using System;

public static class GameActions
{
    public static Action<GameModes> ChangeGameMode;
    public static Action ReloadGameMode;
    public static Action<GameModes> onBeforeGameModeChanged;
    public static Action<GameModes> onAfterGameModeChanged;

    public static Action StartGameMode;
    public static Action onBeforeGameModeStarted;
    public static Action onAfterGameModeStarted;

    public static Action EndGameMode;
    public static Action onBeforeGameModeEnded;
    public static Action onAfterGameModeEnded;

    public static Action<ModuleStatus> SwitchModuleStatus;

    public static Action<int> OnCurrencyChanged;
    public static Action AfterGameLoaded;

}