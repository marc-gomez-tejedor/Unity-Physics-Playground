using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private AllGravityBodies _gravityBodies;

    [Header("Initialitazion order")]
    [SerializeField] private List<MonoBehaviour> rootSystemsScripts;
    [SerializeField] private List<MonoBehaviour> oneDependencyScripts;

    private void Awake()
    {
        Game.Init(_gravityBodies);

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
