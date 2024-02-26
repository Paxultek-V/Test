
public class Player_BounceCannonButton : Player_BounceInteraction
{
    protected override void BounceInterraction()
    {
        base.BounceInterraction();
        
        CannonButton cannonButton = m_hitCollider[0].GetComponent<CannonButton>();

        if (cannonButton == null || cannonButton.IsActive == false)
        {
            OnBounceOnNothing?.Invoke();
            return;
        }

        OnBounceOnObject?.Invoke();
        cannonButton.ShootCannonBall();
    }
}
