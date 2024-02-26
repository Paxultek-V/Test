using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public static System.Action<Obstacle> OnObstacleDead;
    public static System.Action OnPlayerHitObstacle;
    
    private LevelScrollingController m_levelScrollingController;
    private float m_zPosThresholdBeforeReplacing;
    
    
    public void Initialize(LevelScrollingController levelScrollingController)
    {
        m_levelScrollingController = levelScrollingController;
        m_zPosThresholdBeforeReplacing = m_levelScrollingController.ZPosThresholdBeforeReplacing;
    }
    
    public void Bounce()
    {
        OnPlayerHitObstacle?.Invoke();
    }
    
    private void Kill()
    {
        OnObstacleDead?.Invoke(this);
        Destroy(gameObject);
    }
    
    public void MoveObstacle()
    {
        transform.position += Vector3.back * (m_levelScrollingController.CurrentScrollingSpeed * Time.deltaTime);

        if (transform.position.z < m_zPosThresholdBeforeReplacing)
        {
            Kill();
        }
    }
}
