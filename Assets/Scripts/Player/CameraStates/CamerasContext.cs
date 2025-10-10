using System;
using UnityEngine;

[Serializable]
public class OrbitCameraContext
{
    public Camera camera;
    public Transform cameraTransform;
    public Transform focus;
}

[Serializable]
public class TargetLockedCameraContext
{
    public Camera camera;
    public Transform cameraTransform;
    public Transform focus;
}
