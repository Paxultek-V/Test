using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_StartGame : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Button m_button = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_button.interactable)
        {
            GameActions.StartGameMode?.Invoke();
        }
    }
}