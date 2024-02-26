
using UnityEngine;

public class Player_BouncePlatform : Player_BounceInteraction
{
    public static System.Action OnFall;
    
    protected override void OnPlayerBounce()
    {
        Physics.OverlapSphereNonAlloc(transform.position, m_colliderSize, m_hitCollider, m_effectiveLayer);

        if (m_hitCollider[0] == null)
        {
            Debug.Log("fall");
            OnFall?.Invoke();
            return;
        }
        
        BounceInterraction();
    }
    
    protected override void BounceInterraction()
    {
        base.BounceInterraction();
        
        Platform platform = m_hitCollider[0].GetComponent<Platform>();

        if (platform == null)
            return;
        
        OnBounceOnObject?.Invoke();
        platform.Bounce();

        m_hitCollider[0] = null;
    }
}
