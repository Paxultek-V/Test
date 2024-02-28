using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoMistakeController : MonoBehaviour
{
    public static System.Action<int> OnSendMistakeLimit;
    public static System.Action<int> OnSendMistakeCount;
    public static System.Action OnMistakeLimitReached;
    
    [SerializeField] private int m_mistakeLimit = 3;

    private int m_mistakeCount;
    
    private void OnEnable()
    {
        PlayerEvents.OnPlayerBreakCombo += AddMistake;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerBreakCombo -= AddMistake;
    }

    private void Start()
    {
        OnSendMistakeLimit?.Invoke(m_mistakeLimit);
        m_mistakeCount = 0;
        OnSendMistakeCount?.Invoke(m_mistakeCount);
    }

    private void AddMistake()
    {
        m_mistakeCount++;
        OnSendMistakeCount?.Invoke(m_mistakeCount);

        if (m_mistakeCount >= m_mistakeLimit)
        {
            OnMistakeLimitReached?.Invoke();
        }
    }
}
