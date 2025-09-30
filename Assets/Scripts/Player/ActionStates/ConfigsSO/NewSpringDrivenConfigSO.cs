using UnityEngine;

[CreateAssetMenu(menuName = "ActionsConfigs/NewSpringDrivenConfigSO")]
public class NewSpringDrivenConfigSO : ScriptableObject
{
    [Header("Movement tuning")]
    [Range(0f, 100f)]
    public float maxAcceleration = 20f;
    public float maxAirAcceleration = 1f,
          maxClimbAcceleration = 10f,
          maxSwimAcceleration = 5f;


    [Header("Floating Spring Params")]
    [Min(0f)]
    public float rideHeight = 0.5f;
    [Range(0f, 1000f)]
    public float rideSpringStrength = 20f;
    [Range(0f, 100f)]
    public float rideSpringDamper = 20f;


    [Header("Speed caps")]
    [Range(0f, 100f)]
    public float maxSpeed = 10f;
    public float maxClimbSpeed = 2f,
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
    public LayerMask probeMask = -1,
              stairsMask = -1,
              climbMask = -1,
              waterMask = 0;
    [Min(0f)]
    public float downRayDistance = 1f;
    [Min(0f)]
    public float fwdRayDistance = 1f;
    public Vector3 downHalfExtents = new Vector3(1f, 0.01f, 1f);
    public Vector3 fwdHalfExtents = new Vector3(1f, 0.01f, 1f);


    [Header("Angle limits & precomputed values")]
    [Range(0f, 90f)]
    public float maxGroundAngle = 25f,
          maxStairsAngle = 50f;


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
