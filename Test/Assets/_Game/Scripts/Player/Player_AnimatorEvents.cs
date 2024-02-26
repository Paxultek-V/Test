using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_AnimatorEvents : MonoBehaviour
{
    

    public void BroadcastPlayerBounceEvent()
    {
        PlayerEvents.OnPlayerBounce?.Invoke();
    }
}
