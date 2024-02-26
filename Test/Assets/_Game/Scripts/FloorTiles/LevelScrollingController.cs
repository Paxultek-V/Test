using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelScrollingController : MonoBehaviour
{
    [SerializeField] private SmallEnemy m_enemyPrefab = null;
    [SerializeField] private Transform m_enemyParent = null;
    [SerializeField] private Obstacle m_obstaclePrefab = null;
    [SerializeField] private Transform m_obstacleParent = null;
    [SerializeField] private float m_zSpawnPosition = 50f;
    [SerializeField] private float m_xSpawnPositionStep = 5f;
    [SerializeField] private float m_spawnCooldown = 1f;

    [SerializeField] private List<FloorTile> m_floorTileList = null;
    [SerializeField] private float m_baseScrollingSpeed = 10f;
    [SerializeField] private float m_maxScrollingSpeed = 20f;
    [SerializeField] private float m_tileSize = 10;
    [SerializeField] public float m_zPosThresholdBeforeReplacing = -20f;

    private List<float> m_xPositionRowList = new List<float>();
    private List<SmallEnemy> m_smallEnemyList = new List<SmallEnemy>();
    private List<Obstacle> m_obstacleList = new List<Obstacle>();
    private Obstacle m_instantiatedObstacle;
    private SmallEnemy m_instantiatedSmallEnemy;
    private Vector3 m_spawnPositionBuffer;
    private float m_spawnTimer;
    private bool m_isActive;


    public float CurrentScrollingSpeed { get; private set; }

    public float ZPosThresholdBeforeReplacing
    {
        get => m_zPosThresholdBeforeReplacing;
    }

    public float ZPosForReplacing
    {
        get => m_tileSize * m_floorTileList.Count + ZPosThresholdBeforeReplacing;
    }

    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += EnableScrollingLevel;
        SmallEnemy.OnSmallEnemyDead += OnSmallEnemyDead;
        Obstacle.OnObstacleDead += OnObstacleDead;
        PlayerEvents.OnPlayerBounce += OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression += ComputeScrollingSpeed;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= EnableScrollingLevel;
        SmallEnemy.OnSmallEnemyDead -= OnSmallEnemyDead;
        Obstacle.OnObstacleDead -= OnObstacleDead;
        PlayerEvents.OnPlayerBounce -= OnPlayerBounce;
        Controller_BounceCombo.OnSendComboProgression -= ComputeScrollingSpeed;
    }

    private void Start()
    {
        for (int i = 0; i < m_floorTileList.Count; i++)
        {
            m_floorTileList[i].Initialize(this);
        }
        
        m_xPositionRowList.Add(-m_xSpawnPositionStep);
        m_xPositionRowList.Add(0);
        m_xPositionRowList.Add(m_xSpawnPositionStep);
    }

    private void Update()
    {
        MoveFloorTiles();
        MoveEnemies();
        MoveObstacle();
        //ManageEnemySpawn();
    }

    private void ComputeScrollingSpeed(float comboProgression)
    {
        CurrentScrollingSpeed = Mathf.Lerp(m_baseScrollingSpeed, m_maxScrollingSpeed, comboProgression);
    }

    private void OnPlayerBounce()
    {
        List<float> xPositionRowList = new List<float>(m_xPositionRowList);
        
        m_spawnPositionBuffer.z = m_zSpawnPosition;
        
        int randomIndex = Random.Range(0, xPositionRowList.Count);
        m_spawnPositionBuffer.x = xPositionRowList[randomIndex];
        xPositionRowList.RemoveAt(randomIndex);
        
        SpawnEnemy(m_spawnPositionBuffer);

        randomIndex = Random.Range(0, xPositionRowList.Count);
        m_spawnPositionBuffer.x = xPositionRowList[randomIndex];
        xPositionRowList.RemoveAt(randomIndex);
        
        SpawnObstacle(m_spawnPositionBuffer);
    }


    private void SpawnObstacle(Vector3 position)
    {
        if (!m_isActive)
            return;

        m_instantiatedObstacle = Instantiate(m_obstaclePrefab, position, Quaternion.Euler(0f, 180f, 0f),
            m_enemyParent);
        m_instantiatedObstacle.Initialize(this);
        m_obstacleList.Add(m_instantiatedObstacle);
    }
    
    private void SpawnEnemy(Vector3 position)
    {
        if (!m_isActive)
            return;

        m_instantiatedSmallEnemy = Instantiate(m_enemyPrefab, position, Quaternion.Euler(0f, 180f, 0f),
            m_enemyParent);
        m_instantiatedSmallEnemy.Initialize(this);
        m_smallEnemyList.Add(m_instantiatedSmallEnemy);
    }

    private void MoveFloorTiles()
    {
        if (!m_isActive)
            return;

        for (int i = 0; i < m_floorTileList.Count; i++)
        {
            m_floorTileList[i].MoveFloorTile();
        }
    }

    private void MoveEnemies()
    {
        if (!m_isActive)
            return;

        for (int i = 0; i < m_smallEnemyList.Count; i++)
        {
            m_smallEnemyList[i].MoveEnemy();
        }
    }
    
    private void MoveObstacle()
    {
        if (!m_isActive)
            return;

        for (int i = 0; i < m_obstacleList.Count; i++)
        {
            m_obstacleList[i].MoveObstacle();
        }
    }

    private void OnSmallEnemyDead(SmallEnemy deadSmallEnemy)
    {
        m_smallEnemyList.Remove(deadSmallEnemy);
    }

    private void OnObstacleDead(Obstacle deadObstacle)
    {
        m_obstacleList.Remove(deadObstacle);
    }
    
    private void EnableScrollingLevel()
    {
        m_isActive = true;
    }
}