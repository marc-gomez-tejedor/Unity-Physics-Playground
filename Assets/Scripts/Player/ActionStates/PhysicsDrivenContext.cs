using System;
using UnityEngine;

[Serializable]
public class PhysicsDrivenContext
{
    public Rigidbody body;
}

[Serializable]
public class BallVisualContext
{
    public Rigidbody body;
    public Transform ballTransform;
    public MeshRenderer ballMesh;
}
