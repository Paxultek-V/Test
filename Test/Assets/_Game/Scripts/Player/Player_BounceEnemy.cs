
using UnityEngine;

public class Player_BounceEnemy : Player_BounceInteraction
{
    protected override void BounceInterraction()
    {
        base.BounceInterraction();
        
        SmallEnemy smallEnemy = m_buttonCollider[0].GetComponent<SmallEnemy>();

        if (smallEnemy == null || smallEnemy.IsAlive == false)
            return;
        
        OnBounceOnObject?.Invoke();
        smallEnemy.Bounce();
    }
}
