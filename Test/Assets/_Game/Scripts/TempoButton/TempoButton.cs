using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoButton : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_tempoButtonPressedFx = null;



    public void PressTempoButton()
    {
        m_tempoButtonPressedFx.Play();
    }
    
}
