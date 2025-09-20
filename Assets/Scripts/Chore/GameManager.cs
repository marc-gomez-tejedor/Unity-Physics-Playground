using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static void LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(scene, mode);
    }
    public static void LoadScene(int id)
    {
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    }
}
