using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelController_Runner : LevelController
{
    [SerializeField] private Platform m_platformPrefab = null;
    [SerializeField] private Transform m_platformParent = null;

    [SerializeField] private int m_simultaneousPlatformCount = 15;
    [SerializeField] private float m_baseScrollingSpeed = 10f;
    [SerializeField] private float m_maxScrollingSpeed = 20f;

    [SerializeField] private float m_xSpawnPositionEdge = 5f;
    [SerializeField] private float m_zThresholdBeforeKill = -25f;

    [SerializeField] private float m_xPositionOffsetPower = 1f;

    private List<Platform> m_platformList = new List<Platform>();
    private Platform m_instantiatedPlatform;
    private Vector3 m_spawnPositionBuffer;

    private float m_previousSpawnPositionX;

    private float m_zSpawnPosition
    {
        get => m_simultaneousPlatformCount * m_baseScrollingSpeed;
    }

    public float CurrentScrollingSpeed { get; private set; }

    public float ZPosThresholdBeforeKill
    {
        get => m_zThresholdBeforeKill;
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        GameActions.onAfterGameModeStarted += EnableScrollingLevel;
        Platform.OnPlatformDestroyed += OnPlatformDestroyed;
        PlayerEvents.OnPlayerBounce += OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression += ComputeScrollingSpeed;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameActions.onAfterGameModeStarted -= EnableScrollingLevel;
        Platform.OnPlatformDestroyed -= OnPlatformDestroyed;
        PlayerEvents.OnPlayerBounce -= OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression -= ComputeScrollingSpeed;
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
        for (int i = 0; i < m_simultaneousPlatformCount + 1; i++)
        {
            SpawnPlatform(i * m_baseScrollingSpeed);
        }
    }

    private void OnPlayerBounce()
    {
        if (!m_isActive)
            return;

        SpawnPlatform(m_zSpawnPosition, m_previousSpawnPositionX);
    }

    private void EnableScrollingLevel()
    {
        m_isActive = true;
    }

    private void SpawnPlatform(float zPosition, float xPosition = default)
    {
        float newSpawnPositionX = 0f;

        if (xPosition == default)
        {
            newSpawnPositionX = Random.Range(-m_xSpawnPositionEdge, m_xSpawnPositionEdge);
        }
        else
        {
            float previousPositionX = xPosition;
            newSpawnPositionX = previousPositionX + Random.Range(-1f, 1f) * m_xPositionOffsetPower;
            newSpawnPositionX = Mathf.Clamp(newSpawnPositionX, -m_xSpawnPositionEdge, m_xSpawnPositionEdge);
        }

        Vector3 spawnPosition = new Vector3(newSpawnPositionX, 0f, zPosition);
        m_previousSpawnPositionX = spawnPosition.x;

        m_instantiatedPlatform = Instantiate(m_platformPrefab, spawnPosition, Quaternion.identity, m_platformParent);

        m_instantiatedPlatform.Initialize(this);

        m_platformList.Add(m_instantiatedPlatform);

    }

    private void ComputeScrollingSpeed(float comboProgression)
    {
        CurrentScrollingSpeed = Mathf.Lerp(m_baseScrollingSpeed, m_maxScrollingSpeed, comboProgression);
    }

    private void OnPlatformDestroyed(Platform killedPlatform)
    {
        m_platformList.Remove(killedPlatform);
    }

    private void MoveFloorTiles()
    {
        if (!m_isActive)
            return;

        for (int i = 0; i < m_platformList.Count; i++)
        {
            m_platformList[i].MovePlatform();
        }
    }
}