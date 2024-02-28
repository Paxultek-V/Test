using System;
using UnityEngine;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class Player_Movement : ModuleBase
{
    [SerializeField] private float m_pixelToWorldMeter = 200f;

    [SerializeField] private float m_baseMovementSpeed = 15f;

    [SerializeField] private float m_maxMovementSpeed = 30f;
    
    [SerializeField] private float m_levelXBounds = 12f;
    
    
    private Vector3 m_controllerDirection;
    private Vector3 m_playerStartPosition;
    private Vector3 m_desiredPosition;
    private Vector3 m_deadMovementDirection;
    private float m_currentMovementSpeed;
    private float m_desiredXPosition;
    private bool m_isMovementEnabled;
    private bool m_isAlive;
    
    private void OnEnable()
    {
        ControllerComputer.OnSendControllerData += OnSendControllerData;
        Controller.OnTapBegin += SetStartPosition;
        Controller.OnRelease += ResetMovement;
        Boss.OnBossKillPlayer += OnBossKillPlayer;
        Obstacle.OnPlayerHitObstacle += OnPlayerHitObstacle;
        Player_BouncePlatform.OnFall += OnFall;
        TempoMistakeController.OnMistakeLimitReached += OnMistakeLimitReached;
        GameActions.onAfterGameModeStarted += EnableMovement;
        Controller_BounceCombo.OnSendComboProgression += ComputeNewSpeed;
    }

    private void OnDisable()
    {
        ControllerComputer.OnSendControllerData -= OnSendControllerData;
        Controller.OnTapBegin -= SetStartPosition;
        Controller.OnRelease -= ResetMovement;
        Boss.OnBossKillPlayer -= OnBossKillPlayer;
        Obstacle.OnPlayerHitObstacle -= OnPlayerHitObstacle;
        Player_BouncePlatform.OnFall -= OnFall;
        TempoMistakeController.OnMistakeLimitReached -= OnMistakeLimitReached;
        GameActions.onAfterGameModeStarted -= EnableMovement;
        Controller_BounceCombo.OnSendComboProgression -= ComputeNewSpeed;
    }

    private void Start()
    {
        m_isAlive = true;
    }

    protected override void Update()
    {
        base.Update();
        ManageMovement();
        ManageDeadMovement();
    }

    private void EnableMovement()
    {
        m_isMovementEnabled = true;
    }

    private void ComputeNewSpeed(float comboProgression)
    {
        m_currentMovementSpeed = Mathf.Lerp(m_baseMovementSpeed, m_maxMovementSpeed, comboProgression);
    }

    private void OnPlayerHitObstacle()
    {
        Kill(default);
    }
    
    private void OnBossKillPlayer()
    {
        Kill(default);
    }

    private void OnFall()
    {
        Kill(Vector3.down);
    }

    private void OnMistakeLimitReached()
    {
        Kill(default);
    }
    
    private void Kill(Vector3 deathDirection = default)
    {
        m_isAlive = false;

        if (deathDirection == default)
            m_deadMovementDirection =
                (Manager_Camera.Instance.Camera.transform.position - transform.position).normalized;
        else
            m_deadMovementDirection = deathDirection;
        
        Destroy(gameObject, 2f);
    }
    
    private void ManageDeadMovement()
    {
        if(m_isAlive)
            return;
        
        transform.position += m_deadMovementDirection * m_currentMovementSpeed * 5f * Time.deltaTime;
    }
    private void ManageMovement()
    {
        if(!m_isAlive)
            return;
        
        if(!m_isMovementEnabled)
            return;
        
        if (m_controllerDirection == Vector3.zero)
            return;

        m_desiredXPosition = (m_controllerDirection.x / m_pixelToWorldMeter);
        
        m_desiredPosition = m_playerStartPosition + Vector3.right * m_desiredXPosition;
        m_desiredPosition.x = Mathf.Clamp(m_desiredXPosition, -m_levelXBounds / 2f, m_levelXBounds / 2f);
        
        transform.position = Vector3.MoveTowards(transform.position,
            m_desiredPosition,
            m_currentMovementSpeed * Time.deltaTime);
    }

    private void ResetMovement(Vector3 cursorPosition)
    {
        m_desiredXPosition = transform.position.x;
        m_controllerDirection = Vector3.zero;
        m_playerStartPosition = transform.position;
    }
    
    private void SetStartPosition(Vector3 cursorPosition)
    {
        m_playerStartPosition = transform.position;
    }

    private void OnSendControllerData(Vector3 controllerDirection)
    {
        m_controllerDirection = controllerDirection;
    }
}