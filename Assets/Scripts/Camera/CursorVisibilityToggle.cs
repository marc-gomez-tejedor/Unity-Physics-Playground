using Unity.VisualScripting;
using UnityEngine;

public class CursorVisibilityToggle : MonoBehaviour, IInitializable
{
    private bool cursorVisibility = false;

    public void Initialize()
    {
        cursorVisibility = false;
        Cursor.visible = cursorVisibility;
        OnEnable();
    }
    public void CursosVisibilityToggle()
    {
        if (cursorVisibility) cursorVisibility = false;
        else cursorVisibility = true;
    }
    public void ShowCursor()
    {
        cursorVisibility = true;
    }
    public void HideCursor()
    {
        cursorVisibility = false;
    }
    public void OnEnable()
    {
        if (Game.Input != null)
        {
            Game.Input.OnMenu += CursosVisibilityToggle;
        }
    }
    public void OnDisable()
    {
        Game.Input.OnMenu -= CursosVisibilityToggle;
    }
}
