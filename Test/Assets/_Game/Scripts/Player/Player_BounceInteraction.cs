using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BounceInteraction : MonoBehaviour
{
    public static System.Action OnBounceOnObject;
    public static System.Action OnBounceOnNothing;
    
    [SerializeField] protected float m_colliderSize = 2f;

    [SerializeField] protected LayerMask m_effectiveLayer;
    
    protected Collider[] m_buttonCollider;
    
    private void OnEnable()
    {
        PlayerEvents.OnPlayerBounce += OnPlayerBounce;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerBounce -= OnPlayerBounce;
    }

    private void Awake()
    {
        Initialize();
    }

    
    private void Initialize()
    {
        m_buttonCollider = new Collider[1];
    }
    
    protected virtual void OnPlayerBounce()
    {
        Physics.OverlapSphereNonAlloc(transform.position, m_colliderSize, m_buttonCollider, m_effectiveLayer);

        if (m_buttonCollider[0] == null)
        {
            OnBounceOnNothing?.Invoke();
            return;
        }
        
        BounceInterraction();
    }

    protected (bool, T) HasElement<T>() where T : MonoBehaviour
    {
        for (int i = 0; i < m_buttonCollider.Length; i++)
        {
            if(m_buttonCollider[i] == null)
                continue;
            
            T item = m_buttonCollider[i].GetComponent<T>();

            if (item != null)
                return (true, item);
        }

        return (false, null);
    }
    
    protected virtual void BounceInterraction()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, m_colliderSize);
        
    }
}
