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
            rootSystemsScripts[i].GetComponent<IInitializable>().Initialize();
            
        }
        for (int i = 0; i < oneDependencyScripts.Count; i++)
        {
            oneDependencyScripts[i].GetComponent<IInitializable>().Initialize();
        }
    }
}
