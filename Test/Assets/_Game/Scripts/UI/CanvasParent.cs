using UnityEngine;

public class CanvasParent : MonoBehaviour
{
    [field: SerializeField] public Enum_UI_Canvas ParentCanvas { get; private set; }

    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    [field: SerializeField] public bool AlwaysVisible { get; private set; }

    private void Awake()
    {
        if (CanvasGroup == null) CanvasGroup = GetComponent<CanvasGroup>();
        if (CanvasGroup == null) Debug.LogError("CanvasGroup is null");
    }

    public virtual void Show()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
    }


    public virtual void Hide()
    {
        if (AlwaysVisible) return;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.alpha = 0;
    }
}