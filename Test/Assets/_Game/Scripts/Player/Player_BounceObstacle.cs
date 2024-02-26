
using UnityEngine;

public class Player_BounceObstacle : Player_BounceInteraction
{
    protected override void OnPlayerBounce()
    {
        Physics.OverlapSphereNonAlloc(transform.position, m_colliderSize, m_hitCollider, m_effectiveLayer);
        BounceInterraction();
    }
    
    protected override void BounceInterraction()
    {
        base.BounceInterraction();

        (bool, Obstacle) hasObstacle = HasElement<Obstacle>();
        
        if(!hasObstacle.Item1)
            return;
        
        hasObstacle.Item2.Bounce();
    }
}
