using UnityEngine;

[CreateAssetMenu(menuName = "VisualsConfigs/BallVisualsConfigSO")]
public class BallVisualsConfigSO : ScriptableObject
{
    [Header("Ball Parameters")]
    public Transform ball;
    [Min(0.1f)]
    public float ballRadius = 0.5f;


    [Header("Materials")]
    public Material 
        defaultMaterial,
        climbingMaterial,
        swimmingMaterial;


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
