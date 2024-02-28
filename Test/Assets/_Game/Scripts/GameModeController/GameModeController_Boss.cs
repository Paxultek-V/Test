using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss mode", menuName = "Game modes/Boss mode", order = 0)]
public class GameModeController_Boss : GameModeLoader
{
    [SerializeField] private GameObject m_levelPrefab = null;

    private GameObject m_level;
    public bool IsWin { get; private set; }

    protected override void OnEnable()
    {
        Boss.OnBossDead += OnBossDead;
        Boss.OnBossKillPlayer += OnBossKillPlayer;

        ModuleEvents.OnAfterStatus += OnAfterStatus;
    }

    private void OnDisable()
    {
        Boss.OnBossDead -= OnBossDead;
        Boss.OnBossKillPlayer -= OnBossKillPlayer;
        ModuleEvents.OnAfterStatus -= OnAfterStatus;
    }


    private void OnAfterStatus(ModuleStatus status)
    {
        if (status == ModuleStatus.Active)
            UIAccess.Show(Enum_UI_Canvas.Canvas_BossHealth);
    }

    public override IEnumerator IE_Initialize()
    {
        m_level = Instantiate(m_levelPrefab);
        return base.IE_Initialize();
    }


    public override IEnumerator IE_Deinitialize()
    {
        Destroy(m_level);
        return base.IE_Deinitialize();
    }

    private void OnBossKillPlayer()
    {
        IsWin = false;
        GameActions.EndGameMode?.Invoke();
    }

    private void OnBossDead()
    {
        IsWin = true;
        GameActions.EndGameMode?.Invoke();
    }
}