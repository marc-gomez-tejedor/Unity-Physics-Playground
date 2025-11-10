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

[Serializable]
public class NewSpringDrivenContext
{
    public Rigidbody body;
    public Transform raycastTopOrigin;
    public Transform raycastCenterOrigin;
}

[Serializable]
public class ResponsiveSpringDrivenContext
{
    public Rigidbody body;
    public Transform raycastTopOrigin;
    public Transform raycastCenterOrigin;
}

[Serializable]
public class SpringDrivenMothershipContext
{
    public Rigidbody body;
    public Transform raycastTopOrigin;
    public Transform raycastCenterOrigin;
    public Rigidbody mothershipRB;
}
