using System;
using System.Linq;

public static class UIAccess
{
    private static readonly UIManager UIManager = BootLoaderAccess.GetManager<UIManager>();

    public static void Show(this Enum_UI_Canvas enumUICanvas)
    {
        var parent = UIManager.GetCanvasGroup(enumUICanvas);
        parent.Show();
    }

    public static void Hide(this Enum_UI_Canvas enumUICanvas)
    {
        var parent = UIManager.GetCanvasGroup(enumUICanvas);
        
        parent.Hide();
    }

    public static void DisableAll(params Enum_UI_Canvas[] exclude)
    {
        foreach (Enum_UI_Canvas enumUiCanvas in Enum.GetValues(typeof(Enum_UI_Canvas)))
        {
            if (exclude.Contains(enumUiCanvas))
            {
                enumUiCanvas.Show();
                continue;
            }

            enumUiCanvas.Hide();
        }
    }
}