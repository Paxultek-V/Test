using UnityEngine;

public class Controller_BounceCombo : MonoBehaviour
{
    public static System.Action<int> OnSendCombo;
    public static System.Action<float> OnSendComboProgression;

    [SerializeField] private int m_maxComboImpactOnGameplay = 20;

    private float m_comboProgression;
    private int m_currentCombo;
    
    private void OnEnable()
    {
        GameActions.onAfterGameModeStarted += ResetCombo;
        Player_BounceInteraction.OnBounceOnNothing += ResetCombo;
        Player_BounceInteraction.OnBounceOnObject += IncreaseCombo;
    }

    private void OnDisable()
    {
        GameActions.onAfterGameModeStarted -= ResetCombo;
        Player_BounceInteraction.OnBounceOnNothing -= ResetCombo;
        Player_BounceInteraction.OnBounceOnObject -= IncreaseCombo;
    }

    private void ResetCombo()
    {
        m_currentCombo = 0;
        OnSendCombo?.Invoke(m_currentCombo);

        m_comboProgression = Mathf.Clamp01((float)m_currentCombo / m_maxComboImpactOnGameplay);
        OnSendComboProgression?.Invoke(m_comboProgression);
    }

    private void IncreaseCombo()
    {
        m_currentCombo++;
        OnSendCombo?.Invoke(m_currentCombo);

        m_comboProgression = Mathf.Clamp01((float)m_currentCombo / m_maxComboImpactOnGameplay);
        OnSendComboProgression?.Invoke(m_comboProgression);
    }
    
}
