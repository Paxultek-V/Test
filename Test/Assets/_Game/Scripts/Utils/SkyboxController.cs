using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [SerializeField] private Material m_skybox = null;

    private void Awake()
    {
        if (m_skybox != null && m_skybox != RenderSettings.skybox)
            RenderSettings.skybox = m_skybox;
    }
}