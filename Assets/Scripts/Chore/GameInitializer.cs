using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Initialitazion order")]
    [SerializeField] private List<MonoBehaviour> rootSystemsScripts;
    [SerializeField] private List<IInitializable> rootSystemsInitInterfaces;
    [SerializeField] private List<MonoBehaviour> oneDependencyScripts;
    [SerializeField] private List<IInitializable> oneDependencyScriptsInterfaces;

    private void Awake()
    {
        for (int i = 0; i < rootSystemsScripts.Count; i++)
        {
            IInitializable script = (IInitializable)rootSystemsScripts[i];
            script.Initialize();
        }
        for (int i = 0; i < oneDependencyScripts.Count; i++)
        {
            IInitializable script = (IInitializable)oneDependencyScripts[i];
            script.Initialize();
        }
    }
}
