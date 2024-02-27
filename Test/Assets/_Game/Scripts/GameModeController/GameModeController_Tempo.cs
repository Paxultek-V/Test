using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tempo mode", menuName = "Game modes/Tempo mode", order = 0)]
public class GameModeController_Tempo : GameModeLoader
{
    [SerializeField] private GameObject m_levelPrefab = null;

}
