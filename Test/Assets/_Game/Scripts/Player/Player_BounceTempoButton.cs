
public class Player_BounceTempoButton : Player_BounceInteraction
{
    protected override void BounceInterraction()
    {
        base.BounceInterraction();
        
        TempoButton tempoButton = m_hitCollider[0].GetComponent<TempoButton>();

        if (tempoButton == null)
        {
            OnBounceOnNothing?.Invoke();
            return;
        }

        OnBounceOnObject?.Invoke();
        tempoButton.PressTempoButton();
    }
}
