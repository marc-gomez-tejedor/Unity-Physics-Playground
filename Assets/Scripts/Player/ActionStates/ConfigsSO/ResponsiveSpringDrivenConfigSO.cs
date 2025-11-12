using UnityEngine;

[CreateAssetMenu(menuName = "ActionsConfigs/ResponsiveSpringDrivenConfigSO")]
public class ResponsiveSpringDrivenConfigSO : ScriptableObject
{
    [Header("Movement tuning")]
    [Range(0f, 200f)]
    public float maxAcceleration = 150f;
    public float dotAccelerationFactor = 1f;
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
    [Range(0f, 20f)]
    public float jumpHeight = 2f;
    [Range(0, 5)]
    public int maxAirJumps = 0;
    [Range(0, 20)]
    public int snapStepsThreshold = 10;
    [Range(0f, 1000f)]
    public float dashSpeed = 1000f;


    [Header("Can crouch")]
    public bool crouches;
    [Range(0f, 100f)]
    public float crouchAcceleration;


    [Header("Probing & snap params")]
    public LayerMask probeMask = -1,
              climbMask = -1,
              waterMask = 0;
    [Min(0f)]
    public float downRayDistance = 1f;
    [Min(0f)]
    public float fwdRayDistance = 1f;
    [Min(0f)]
    public float downBoxDistance = 1f;
    [Min(0f)]
    public float fwdSphereDistance = 1f;
    public Vector3 downHalfExtents = new Vector3(1f, 0.01f, 1f);
    [Min(0.1f)]
    public float fwdSphereRadius = 1f;


    [Header("Angle limits & precomputed values")]
    [Range(0f, 90f)]
    public float maxGroundAngle = 89f;
    [Range(90f, 180f)]
    public float maxClimbAngle = 140f;


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
