using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        Game.LoadScene(scene);
    }
    public void LoadMenuScene()
    {
        Game.LoadMenuScene();
    }
    public void LoadTestScene()
    {
        Game.LoadTestScene();
    }
    public void LoadOrbits()
    {
        Game.LoadOrbits();
    }
    public void LoadSpaceshipScene()
    {
        Game.LoadSpaceshipScene();
    }
    public void LoadSpringlikeScene()
    {
        Game.LoadSpringlikeScene();
    }
    public void LoadMiniPlanetsScene()
    {
        Game.LoadMiniPlanetsScene();
    }
    public void LoadSwimmingScene()
    {
        Game.LoadSwimmingScene();
    }
    public void LoadInteractiveScene()
    {
        Game.LoadInteractiveScene();
    }
}
