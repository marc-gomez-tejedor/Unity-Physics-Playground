using UnityEngine;

[System.Serializable]
public class PlayerPhysicsContext
{
    public Rigidbody ConnectedBody;
    public Rigidbody PreviousConnectedBody;
    public Vector3 LastConnectionVelocity;
    public Vector3 LocalGroundNormal;
    public Vector3 LastContactNormal;
    public Vector3 LastSteepNormal;
}
