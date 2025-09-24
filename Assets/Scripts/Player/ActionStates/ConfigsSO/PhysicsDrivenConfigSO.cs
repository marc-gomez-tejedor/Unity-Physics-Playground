using UnityEngine;

[CreateAssetMenu(menuName = "ActionsConfigs/PhysicsDrivenConfigSO")]
public class PhysicsDrivenConfigSO : ScriptableObject
{
    [Header("Movement tuning")]
    [Range(0f, 100f)]
    public float maxAcceleration = 20f,
          maxAirAcceleration = 1f,
          maxClimbAcceleration = 10f,
          maxSwimAcceleration = 5f;


    [Header("Speed caps")]
    [Range(0f, 100f)]
    public float maxSpeed = 10f,
          maxClimbSpeed = 2f,
          maxSwimSpeed = 5f;


    [Header("Jump params")]
    [Range(0f, 10f)]
    public float jumpHeight = 2f;
    [Range(0, 5)]
    public int maxAirJumps = 0;


    [Header("Climb tuning")]
    [Range(90f, 180f)]
    public float maxClimbAngle = 140f;


    [Header("Probing & snap params")]
    public float maxSnapSpeed = 100f;
    [Min(0f)]
    public float probeDistance = 1f;
    public LayerMask probeMask = -1,
              stairsMask = -1,
              climbMask = -1,
              waterMask = 0;


    [Header("Angle limits & precomputed values")]
    [Range(0f, 90f)]
    public float maxGroundAngle = 25f,
          maxStairsAngle = 50f;
    public float minGroundDotProduct,
          minStairsDotProduct,
          minClimbDotProduct;


    [Header("Water-specific tuning")]
    public float submergenceOffset = 0.5f;
    [Min(0.1f)]
    public float submergenceRange = 1f;
    [Range(0f, 10f)]
    public float waterDrag = 1f;
    [Min(0f)]
    public float buoyancy = 1f;
    [Range(0.01f, 1f)]
    public float swimThreshold = 0.5f;
}
