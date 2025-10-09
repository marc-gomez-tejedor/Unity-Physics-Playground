using UnityEngine;

[CreateAssetMenu(menuName = "VisualsConfigs/CapsuleVisualsConfigSO")]
public class CapsuleVisualsConfigSO : ScriptableObject
{
    public GameObject prefab;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material climbingMaterial;
    public Material swimmingMaterial;
}
