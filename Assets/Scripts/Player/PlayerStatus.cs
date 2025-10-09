using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public GameObject visualObject;
    public GameObject weaponObject;
    public GravityQuerySettings playerGravityQuery;
    public bool OnGround;
    public bool OnSteep;
    public bool Climbing;
    public bool InWater;
    public bool Swimming;
    public int StepsSinceLastGrounded;
    public int StepsSinceLastJump;
    public float Submergence;
    public Vector3 UpAxis;
    public Vector3 ForwardAxis;
}

[System.Serializable]
public class PlayerContactStatus
{
    public Rigidbody ConnectedBody;
    public Rigidbody PreviousConnectedBody;
    public Vector3 LastConnectionVelocity;
    public Vector3 LastContactNormal;
    public Vector3 LastSteepNormal;
}

