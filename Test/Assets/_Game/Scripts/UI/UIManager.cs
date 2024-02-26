using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase
{
    public Dictionary<Enum_UI_Canvas, CanvasParent> _canvasGroups;
    
    public CanvasParent GetCanvasGroup(Enum_UI_Canvas enumUiCanvas)
    {
        return _canvasGroups[enumUiCanvas];
    }
}
