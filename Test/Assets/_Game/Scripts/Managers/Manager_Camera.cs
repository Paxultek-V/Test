using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Camera : MonoBehaviour
{
    public static Manager_Camera Instance;

    public Camera Camera = null;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
