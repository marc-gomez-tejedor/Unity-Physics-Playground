using UnityEngine;

[CreateAssetMenu(menuName = "WeaponsConfigs/DefaultWeaponsConfigSO")]
public class DefaultWeaponsConfigSO : ScriptableObject
{
    [Min(0)]
    public float maximumDistance;
    public LayerMask mask;
}
