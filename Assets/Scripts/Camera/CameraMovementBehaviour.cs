using Unity.VisualScripting;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour, IInitializable
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;


    [Header("Parameters")]
    [SerializeField] private float speed;
    private Vector2 look = Vector2.zero;
    private bool cursorVisibility = false;

    public void Initialize()
    {
        cursorVisibility = false;
        Cursor.visible = cursorVisibility;
        OnEnable();
    }
    public void Move(Vector2 input)
    {
        look += input * speed;

        look.y = Mathf.Clamp(look.y, -85f, 85f);

        playerTransform.localEulerAngles = new Vector3 (0, look.x, 0);
        cameraTransform.localEulerAngles = new Vector3(-look.y, 0, 0);
        Debug.Log($"look: {look}");

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
