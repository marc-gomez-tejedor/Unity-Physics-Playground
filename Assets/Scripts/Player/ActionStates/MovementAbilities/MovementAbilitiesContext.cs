using System;
using UnityEngine;

[Serializable]
public class DashContext
{
    public Rigidbody body;
    public Transform raycastTopOrigin;
    public Transform raycastCenterOrigin;
    public Rigidbody mothershipRB;
}
