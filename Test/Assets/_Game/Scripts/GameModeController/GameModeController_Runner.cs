using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Runner mode", menuName = "Game modes/Runner mode", order = 0)]
public class GameModeController_Runner : GameModeLoader
{
    [SerializeField] private GameObject m_levelPrefab = null;

    private GameObject m_level;
    
    protected override void OnEnable()
    {
        Obstacle.OnPlayerHitObstacle += OnPlayerHitObstacle;
    }

    private void OnDisable()
    {
        Obstacle.OnPlayerHitObstacle -= OnPlayerHitObstacle;
    }
    
    private void OnPlayerHitObstacle()
    {
        GameActions.EndGameMode?.Invoke();
    }
    
    public override IEnumerator IE_Initialize()
    {
        m_level = Instantiate(m_levelPrefab);
        return base.IE_Initialize();
    }


    public override IEnumerator IE_Deinitialize()
    {
        Destroy(m_level);
        return base.IE_Deactivate();
    }
}
