using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public static System.Action<TargetButton> OnTargetButtonDestroyed;
    
    [SerializeField] private GameObject m_visual = null;

    [SerializeField] private Collider m_detectionCollider = null;
    
    private LevelController_Tempo m_levelControllerTempo;
    private float m_zPosThresholdBeforeKill;
    
    public void Initialize(LevelController_Tempo levelControllerTempo)
    {
        m_levelControllerTempo = levelControllerTempo;
        m_zPosThresholdBeforeKill = m_levelControllerTempo.ZPosThresholdBeforeKill;
    }

    public void Detect()
    {
        m_visual.SetActive(false);
        m_detectionCollider.enabled = false;
    }
    
    public void Kill()
    {
        OnTargetButtonDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    
    public void MoveTargetButton()
    {
        transform.position += Vector3.back * (m_levelControllerTempo.CurrentScrollingSpeed * Time.deltaTime);

        if (transform.position.z < m_zPosThresholdBeforeKill)
        {
            Kill();
        }
    }
}
