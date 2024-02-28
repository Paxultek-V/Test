
using UnityEngine;

public class Player_BounceTempoButton : Player_BounceInteraction
{
    protected override void OnPlayerBounce()
    {
        Physics.OverlapSphereNonAlloc(transform.position, m_colliderSize, m_hitCollider, m_effectiveLayer);
        BounceInterraction();
    }
    
    protected override void BounceInterraction()
    {
        base.BounceInterraction();
        
        TempoButton tempoButton = m_hitCollider[0].GetComponent<TempoButton>();

        tempoButton.PressTempoButton();
    }
}
