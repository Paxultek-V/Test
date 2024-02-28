using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Mistakes : MonoBehaviour
{
    [SerializeField] private GameObject m_mistakeImagePrefab = null;
    [SerializeField] private Transform m_mistakeImageParent = null;
    
    
    private List<GameObject> m_mistakeImageList = new List<GameObject>();
    private GameObject m_instantiatedMistakeImage;

    private void OnEnable()
    {
        TempoMistakeController.OnSendMistakeLimit += CreateMistakeImages;
        TempoMistakeController.OnSendMistakeCount += UpdateMistakesCount;
    }

    private void OnDisable()
    {
        TempoMistakeController.OnSendMistakeLimit -= CreateMistakeImages;
        TempoMistakeController.OnSendMistakeCount -= UpdateMistakesCount;
    }


    private void CreateMistakeImages(int mistakeLimit)
    {
        for (int i = 0; i < m_mistakeImageList.Count; i++)
        {
            Destroy(m_mistakeImageList[i]);
        }
        
        m_mistakeImageList.Clear();

        for (int i = 0; i < mistakeLimit; i++)
        {
            m_instantiatedMistakeImage = Instantiate(m_mistakeImagePrefab, m_mistakeImageParent);
            m_mistakeImageList.Add(m_instantiatedMistakeImage);
        }
    }


    private void UpdateMistakesCount(int mistakesCount)
    {
        for (int i = 0; i < m_mistakeImageList.Count; i++)
        {
            if (i > m_mistakeImageList.Count)
                return;

            m_mistakeImageList[i].SetActive(i < mistakesCount);
        }
    }
}