using UnityEngine;

[CreateAssetMenu(menuName = "CameraConfigs/TargetLockedCameraConfigSO")]
public class TargetLockedCameraConfigSO : ScriptableObject
{
    [Range(0f, 10f)]
    public float verticalOffset;

    public LayerMask obstructionMask = -1;

    [Range(1f, 20f)]
    public float distance = 5f;
}
