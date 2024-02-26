using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _Tester : MonoBehaviour
{
    public float Step = 0.2f;

    public List<float> list = new List<float>();

    private void Start()
    {
        float value = 0;
        for (int i = 0; i < 25; i++)
        {
            value = Mathf.Sin(i * Step);
            list.Add(value);
        }
    }
}