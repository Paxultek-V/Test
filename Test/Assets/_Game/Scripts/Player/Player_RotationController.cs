using System;
using UnityEngine;

public class Player_RotationController : ModuleBase
{
    [SerializeField] private Transform m_controlledTransform = null;

    [SerializeField] private float m_distanceToOneCompleteRotationRatio = 4f;

    [SerializeField] private bool m_isMovingForward = false;

    private Vector3 m_startPosition;
    private Vector3 m_desiredRotationAngle;
    private Quaternion m_desiredRotation;
    private bool m_canRotate;
    
    private float m_xDiff
    {
        get => transform.position.x - m_startPosition.x;
    }

    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += EnableRotation;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= EnableRotation;
    }

    private void Start()
    {
        m_startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        UpdateRotation();
    }

    private void EnableRotation()
    {
        m_canRotate = true;
    }
    
    private void UpdateRotation()
    {
        if(!m_canRotate)
            return;
        
        m_desiredRotationAngle.y = 0;
        m_desiredRotationAngle.z = m_xDiff * 360f / m_distanceToOneCompleteRotationRatio;

        if (m_isMovingForward)
            m_desiredRotationAngle.x -= 360 * Time.deltaTime;

        m_desiredRotation = Quaternion.Euler(m_desiredRotationAngle);

        m_controlledTransform.localRotation = m_desiredRotation;
    }
}