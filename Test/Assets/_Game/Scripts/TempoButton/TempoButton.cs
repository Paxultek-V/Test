using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoButton : MonoBehaviour
{
    public static System.Action OnDetectNoTargetButton;
    public static System.Action<TargetButton> OnDetectTargetButton;
    
    [SerializeField] private ParticleSystem m_tempoButtonPressedFx = null;
    
    [SerializeField] private ParticleSystem m_targetButtonDetectordFx = null;

    [SerializeField] private LayerMask m_effectiveLayer = 0;

    [SerializeField] private Transform m_detectionPosition = null;

    [SerializeField] private float m_detectionRange = 1f;
    
    private Collider[] m_hitCollider;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_hitCollider = new Collider[1];
    }
    
    
    public void PressTempoButton()
    {
        m_tempoButtonPressedFx.Play();
        TryDetectTargetButton();
    }

    private void TryDetectTargetButton()
    {
        Physics.OverlapSphereNonAlloc(m_detectionPosition.position, m_detectionRange, m_hitCollider, m_effectiveLayer);

        if (m_hitCollider[0] == null)
        {
            PlayerEvents.OnPlayerBreakCombo?.Invoke();
            return;
        }
        
        TargetButton targetButton = m_hitCollider[0].GetComponent<TargetButton>();
        targetButton.Detect();
        PlayerEvents.OnPlayerIncreaseCombo?.Invoke();
        m_targetButtonDetectordFx.Play();
        
        OnDetectTargetButton?.Invoke(targetButton);
        
        m_hitCollider[0] = null;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_detectionPosition.position, m_detectionRange);
    }
    
}
