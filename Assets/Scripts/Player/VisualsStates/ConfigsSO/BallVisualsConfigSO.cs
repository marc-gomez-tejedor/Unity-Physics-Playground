using UnityEngine;

[CreateAssetMenu(menuName = "VisualsConfigs/BallVisualsConfigSO")]
public class BallVisualsConfigSO : ScriptableObject
{
    public GameObject prefab;

    [Min(0.1f)]
    public float ballRadius = 0.5f;


    [Header("Materials")]
    public Material defaultMaterial;
    public Material climbingMaterial;
    public Material swimmingMaterial;


    [Header("Rotation Parameters")]
    [Min(0f)]
    public float ballAlignSpeed = 180f;
    public bool ballCanReverse = false;
    public float
        ballGroundRotation = 1f,
        ballClimbingRotation = 1f,
        ballAirRotation = 0.5f,
        ballSwimRotation = 2f;
}
