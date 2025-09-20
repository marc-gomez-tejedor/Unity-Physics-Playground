using Unity.VisualScripting;
using UnityEngine;

public class UIToggle : MonoBehaviour, IInitializable
{
    [SerializeField]
    GameObject UIObject;
    public bool active = false;

    public void Initialize()
    {
        UIObject.SetActive(active);
        OnEnable();
    }
    void ToggleOnOff()
    {
        Debug.Log($"active = {active}");
        UIObject.SetActive(!active);
        active = !active;
    }
    void OnEnable()
    {
        if (Game.Input != null)
        {
            Game.Input.OnMenu -= ToggleOnOff;
            Game.Input.OnMenu += ToggleOnOff;
        }
    }
    void OnDisable()
    {
        Game.Input.OnMenu -= ToggleOnOff;
    }
}
