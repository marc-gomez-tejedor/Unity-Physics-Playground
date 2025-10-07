using UnityEngine;

[CreateAssetMenu(menuName = "CameraConfigs/OrbitCameraConfigSO")]
public class OrbitCameraConfigSO : ScriptableObject
{
    [Range(0f, 10f)]
    public float verticalOffset;

    public LayerMask obstructionMask = -1;

    [Range(1f, 20f)]
    public float distance = 5f;
    [Min(0f)]
    public float focusRadius = 1f;
    [Range(0f, 1f)]
    public float focusCentering = 0.5f;

    [Range(1f, 360f)]
    public float rotationSpeed = 90f;
    [SerializeField, Range(-89f, 89f)]
    public float minVerticalAngle = -30f, maxVerticalAngle = 60f;

    [Min(0f)]
    public float alignDelay = 5f;
    [Range(0f, 90f)]
    public float alignSmoothRange = 45f;

    [Min(0f)]
    public float upAlignmentSpeed = 360f;
}
