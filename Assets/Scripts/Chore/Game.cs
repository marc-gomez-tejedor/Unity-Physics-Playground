using UnityEngine;
using UnityEngine.SceneManagement;

public static class Game
{
    public static AllGravityBodies Gravity { get; private set; }
    public static InputManager Input { get; private set; }

    public static void Init(AllGravityBodies gravity)
    {
        Gravity = gravity;
    }

    public static void Init(InputManager input)
    {
        Input = input;
    }
    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public static void LoadMenuScene()
    {

    }
    public static void LoadTestScene()
    {

    }
    public static void LoadOrbits()
    {

    }
    public static void LoadSpaceshipScene()
    {

    }
    public static void LoadSpringlikeScene()
    {

    }
    public static void LoadMiniPlanetsScene()
    {

    }
    public static void LoadSwimmingScene()
    {

    }
    public static void LoadInteractiveScene()
    {
    }
}
