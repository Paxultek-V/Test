using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class TargetButtonInfo
{
    public TargetButton TargetButtonPrefab;
    public Transform SpawnPosition;
}

public class LevelController_Tempo : LevelController
{
    [SerializeField] private List<TargetButtonInfo> m_targetButtonInfoList = null;
    [SerializeField] private Transform m_targetButtonParent = null;

    [SerializeField] private int m_simultaneousTargetButtonCount = 15;
    [SerializeField] private float m_baseScrollingSpeed = 10f;
    [SerializeField] private float m_maxScrollingSpeed = 20f;
    [SerializeField] private float m_zSpawnOffset = 0.5f;

    [SerializeField] private float m_zThresholdBeforeKill = -25f;


    private List<TargetButton> m_targetButtonList = new List<TargetButton>();
    private TargetButton m_instantiatedTargetButton;
    private Vector3 m_spawnPositionBuffer;


    public float CurrentScrollingSpeed { get; private set; }

    public float ZPosThresholdBeforeKill
    {
        get => m_zThresholdBeforeKill;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameActions.onAfterGameModeStarted += EnableScrollingLevel;
        TargetButton.OnTargetButtonDestroyed += OnTargetButtonDestroyed;
        PlayerEvents.OnPlayerBounce += OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression += ComputeScrollingSpeed;
        TempoMistakeController.OnMistakeLimitReached += OnMistakeLimitReached;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameActions.onAfterGameModeStarted -= EnableScrollingLevel;
        TargetButton.OnTargetButtonDestroyed -= OnTargetButtonDestroyed;
        PlayerEvents.OnPlayerBounce -= OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression -= ComputeScrollingSpeed;
        TempoMistakeController.OnMistakeLimitReached -= OnMistakeLimitReached;
    }

    protected override void Start()
    {
        base.Start();
        InitializeLevel();
    }

    protected override void Update()
    {
        base.Update();
        MoveFloorTiles();
    }

    private void InitializeLevel()
    {
        for (int i = 1; i < m_simultaneousTargetButtonCount + 1; i++)
        {
            float zPosition = i * m_baseScrollingSpeed;
            
            if(zPosition > m_targetButtonInfoList[0].SpawnPosition.position.z)
                return;
            
            SpawnTargetButton(zPosition);
        }
    }

    private void OnMistakeLimitReached()
    {
        m_isActive = false;

        for (int i = 0; i < m_targetButtonList.Count; i++)
        {
            m_targetButtonList[i].Kill();
        }
    }
    
    private void OnPlayerBounce()
    {
        if (!m_isActive)
            return;

        SpawnTargetButton();
    }

    private void EnableScrollingLevel()
    {
        m_isActive = true;
    }

    private void SpawnTargetButton(float zPosition = default)
    {
        int randomTargetButtonInfoIndex = Random.Range(0, m_targetButtonInfoList.Count);

        TargetButton targetButtonToSpawn = m_targetButtonInfoList[randomTargetButtonInfoIndex].TargetButtonPrefab;
        Vector3 spawnPosition = m_targetButtonInfoList[randomTargetButtonInfoIndex].SpawnPosition.position;

        if (zPosition != default)
            spawnPosition.z = zPosition;

        spawnPosition.z += m_zSpawnOffset;

        m_instantiatedTargetButton =
            Instantiate(targetButtonToSpawn, spawnPosition, Quaternion.identity, m_targetButtonParent);

        m_instantiatedTargetButton.Initialize(this);

        m_targetButtonList.Add(m_instantiatedTargetButton);
    }

    private void ComputeScrollingSpeed(float comboProgression)
    {
        CurrentScrollingSpeed = Mathf.Lerp(m_baseScrollingSpeed, m_maxScrollingSpeed, comboProgression);
    }

    private void OnTargetButtonDestroyed(TargetButton killedButton)
    {
        m_targetButtonList.Remove(killedButton);
    }

    private void MoveFloorTiles()
    {
        if (!m_isActive)
            return;

        for (int i = 0; i < m_targetButtonList.Count; i++)
        {
            m_targetButtonList[i].MoveTargetButton();
        }
    }
}