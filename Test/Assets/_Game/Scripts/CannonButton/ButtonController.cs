using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private List<CannonButton> m_cannonButtonList = null;

    [SerializeField] private int m_maxButtonActivated = 4;
    
    [SerializeField] private Boss m_boss;
    
    private Coroutine m_buttonsManagementCoroutine;
    private List<CannonButton> m_randomCannonButtonsList;
    private CannonButton m_blackListCannonButton;
    

    private void OnEnable()
    {
        CannonButton.OnCannonButtonPressed += OnCannonButtonPressed;
        GameActions.onAfterGameModeStarted += EnableButtons;
        Boss.OnBossDead += DisableCannonButtons;
    }

    private void OnDisable()
    {
        CannonButton.OnCannonButtonPressed -= OnCannonButtonPressed;
        GameActions.onAfterGameModeStarted -= EnableButtons;
        Boss.OnBossDead -= DisableCannonButtons;
    }

    private void Start()
    {
        InitializeCannonButtons();
    }

    private void EnableButtons()
    {
        ActivateRandomButtons();
    }

    private void DisableCannonButtons()
    {
        for (int i = 0; i < m_cannonButtonList.Count; i++)
        {
            m_cannonButtonList[i].DeactivateButton();
            m_cannonButtonList[i].HideVisuals();
        }
    }
    
    private void InitializeCannonButtons()
    {
        for (int i = 0; i < m_cannonButtonList.Count; i++)
        {
            m_cannonButtonList[i].Initialize(m_boss);
            m_cannonButtonList[i].DeactivateButton();
            m_cannonButtonList[i].HideVisuals();
        }
    }

    private void OnCannonButtonPressed(CannonButton cannonButton)
    {
        m_randomCannonButtonsList.Remove(cannonButton);

        if (m_randomCannonButtonsList.Count <= 0)
        {
            m_blackListCannonButton = cannonButton;
            ActivateRandomButtons();
        }
    }

    private void ActivateRandomButtons()
    {
        int buttonsToActivateCount = Random.Range(1, m_maxButtonActivated);
        m_randomCannonButtonsList = new List<CannonButton>(m_cannonButtonList);
        
        m_randomCannonButtonsList.Remove(m_blackListCannonButton);
        
        while(buttonsToActivateCount < m_randomCannonButtonsList.Count)
        {
            int randomIndex = Random.Range(0, m_randomCannonButtonsList.Count);
            m_randomCannonButtonsList.RemoveAt(randomIndex);
        }

        for (int i = 0; i < m_randomCannonButtonsList.Count; i++)
        {
            m_randomCannonButtonsList[i].ActivateButton();
        }
    }
}