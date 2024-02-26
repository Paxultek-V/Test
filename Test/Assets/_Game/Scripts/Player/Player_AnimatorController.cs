using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator m_animator = null;
    
    
    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += StartBouncing;
        Controller_BounceCombo.OnSendComboProgression += OnSendComboProgression;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= StartBouncing;
        Controller_BounceCombo.OnSendComboProgression -= OnSendComboProgression;
    }

    private void StartBouncing()
    {
        m_animator.SetTrigger("StartBounce");
    }

    private void OnSendComboProgression(float comboProgression)
    {
        UpdateAnimatorSpeed(1 + comboProgression);
    }

    private void UpdateAnimatorSpeed(float speed)
    {
        m_animator.speed = speed;
    }
}
