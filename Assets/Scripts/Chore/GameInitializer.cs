using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private AllGravityBodies gravityBodies;
    [SerializeField] private InputManager input;

    [Header("Initialitazion order")]
    [SerializeField] private List<MonoBehaviour> rootSystemsScripts;
    [SerializeField] private List<MonoBehaviour> oneDependencyScripts;

    private void Awake()
    {
        Game.Init(gravityBodies, input);

        for (int i = 0; i < rootSystemsScripts.Count; i++)
        {
            MonoBehaviour script = rootSystemsScripts[i];
            if (script is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }
        for (int i = 0; i < oneDependencyScripts.Count; i++)
        {
            MonoBehaviour script = oneDependencyScripts[i];
            if (script is IInitializable initializable)
            {
                initializable.Initialize();
            }
        }
    }
}
