using UnityEngine;

[CreateAssetMenu(menuName = "WeaponsConfigs/DefaultWeaponsConfigSO")]
public class DefaultWeaponsConfigSO : ScriptableObject
{
    public GameObject prefab;

    [Min(0)]
    public float maximumDistance;
    public LayerMask mask;
}
