using Unity.VisualScripting;
using UnityEngine;

public class CursorVisibilityToggle : MonoBehaviour, IInitializable
{
    private bool cursorVisibility = false;
    
    public void Initialize()
    {
        cursorVisibility = false;
        Cursor.visible = cursorVisibility;
    }
    public void CursosVisibilityToggle()
    {
        if (cursorVisibility) HideCursor();
        else ShowCursor();
    }
    public void ShowCursor()
    {
        cursorVisibility = true;
        Cursor.visible = cursorVisibility;

    }
    public void HideCursor()
    {
        cursorVisibility = false;
        Cursor.visible = cursorVisibility;
    }
}
