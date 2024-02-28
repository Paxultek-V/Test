using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tempo mode", menuName = "Game modes/Tempo mode", order = 0)]
public class GameModeController_Tempo : GameModeLoader
{
    [SerializeField] private GameObject m_levelPrefab = null;


    private GameObject m_level;


    private void OnAfterStatus(ModuleStatus status)
    {
        if (status == ModuleStatus.Active)
            UIAccess.Show(Enum_UI_Canvas.Canvas_TempoMistakes);
    }

    private void OnMistakeLimitReached()
    {
        GameActions.EndGameMode?.Invoke();
    }

    public override IEnumerator IE_Initialize()
    {
        TempoMistakeController.OnMistakeLimitReached += OnMistakeLimitReached;
        ModuleEvents.OnAfterStatus += OnAfterStatus;

        m_level = Instantiate(m_levelPrefab);
        return base.IE_Initialize();
    }


    public override IEnumerator IE_Deinitialize()
    {
        TempoMistakeController.OnMistakeLimitReached -= OnMistakeLimitReached;
        ModuleEvents.OnAfterStatus -= OnAfterStatus;

        Destroy(m_level);
        return base.IE_Deactivate();
    }
}