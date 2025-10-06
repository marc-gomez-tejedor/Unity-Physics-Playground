using UnityEngine;
using UnityEngine.Events;

public class GamePause : MonoBehaviour, IInitializable
{
    public UnityEvent OnStart, OnPause, OnResume;
    bool running = true;

    public void Initialize()
    {
        OnStart.AddListener(() => Debug.Log("OnStart fired!"));
        OnPause.AddListener(() => Debug.Log("OnPause fired!"));
        OnResume.AddListener(() => Debug.Log("OnResume fired!"));

        OnStart.Invoke();
        OnEnable();
    }
    void ToggleOnOff()
    {
        if (running) OnPause.Invoke();
        else OnResume.Invoke();
        running = !running;
    }
    public void FreezeTime()
    {
        Time.timeScale = 0f;
    }
    public void UnFreezeTime()
    {
        Time.timeScale = 1f;
    }
    void OnEnable()
    {
        if (Game.Input)
        {
            Game.Input.OnMenu -= ToggleOnOff;
            Game.Input.OnMenu += ToggleOnOff;
        }
    }
    void OnDisable()
    {
        if (Game.Input) Game.Input.OnMenu -= ToggleOnOff;
    }

}
