using System;
using UnityEngine;

[Serializable]
public class PhysicsDrivenContext
{
    public Rigidbody body;
}

[Serializable]
public class SpringDrivenContext
{
    public Rigidbody body;
    public Transform raycastOrigin;
}

