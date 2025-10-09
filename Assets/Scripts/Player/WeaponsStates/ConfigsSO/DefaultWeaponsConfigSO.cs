using System;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponsConfigs/DefaultWeaponsConfigSO")]
public class DefaultWeaponsConfigSO : ScriptableObject
{
    public GameObject prefab;

    [Min(0)]
    public float maximumDistance;
    public LayerMask layerMask;
    public GravityType ExcludeMask = GravityType.GravityCastedByPlayer;
    public GravityType IncludeMask = GravityType.GravityRayByPlayer;
}
