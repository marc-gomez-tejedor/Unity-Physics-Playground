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
    public static void LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(scene, mode);
    }
    public static void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public static void LoadTestScene()
    {
        SceneManager.LoadScene("", LoadSceneMode.Single);
    }
    public static void LoadOrbits()
    {
        SceneManager.LoadScene("OrbitsDemo", LoadSceneMode.Single);
    }
    public static void LoadSpaceshipScene()
    {
        SceneManager.LoadScene("MothershipInterior", LoadSceneMode.Single);
    }
    public static void LoadSpringlikeScene()
    {
        SceneManager.LoadScene("SpringBasedCharacterDemo", LoadSceneMode.Single);
    }
    public static void LoadMiniPlanetsScene()
    {
        SceneManager.LoadScene("MiniPlanetsSphereScene", LoadSceneMode.Single);
    }
    public static void LoadSwimmingScene()
    {
        SceneManager.LoadScene("", LoadSceneMode.Single);
    }
    public static void LoadGravityBoxScene()
    {
        SceneManager.LoadScene("BoxTerrainScene", LoadSceneMode.Single);
    }
    public static void LoadInteractiveScene()
    {
        SceneManager.LoadScene("InteractiveTerrainSphereScene", LoadSceneMode.Single);
    }
}
