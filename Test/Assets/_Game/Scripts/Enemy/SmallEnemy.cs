using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    public static System.Action<SmallEnemy> OnSmallEnemyDead;
    
    [SerializeField] private SkinnedMeshRenderer m_meshRenderer = null;

    private LevelScrollingController m_levelScrollingController;
    private float m_zPosThresholdBeforeReplacing;
    
    public bool IsAlive { get; private set; } 
    
    public void Initialize(LevelScrollingController levelScrollingController)
    {
        IsAlive = true;
        m_levelScrollingController = levelScrollingController;
        m_zPosThresholdBeforeReplacing = m_levelScrollingController.ZPosThresholdBeforeReplacing;
        m_meshRenderer.SetBlendShapeWeight(0, 0f);
    }

    public void Bounce()
    {
        m_meshRenderer.SetBlendShapeWeight(0, 100f);
        IsAlive = false;
    }

    private void Kill()
    {
        IsAlive = false;
        
        OnSmallEnemyDead?.Invoke(this);
        Destroy(gameObject);
    }

    public void MoveEnemy()
    {
        transform.position += Vector3.back * (m_levelScrollingController.CurrentScrollingSpeed * Time.deltaTime);

        if (transform.position.z < m_zPosThresholdBeforeReplacing)
        {
            Kill();
        }
    }
}
