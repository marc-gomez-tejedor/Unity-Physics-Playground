using UnityEngine;

public enum enumState
{
    None = 0,
    Idle = 1,
    Running = 2,
    Swimming = 3,
    Climbing = 4,
    Jumping = 5,
    Dashing = 6,
    Falling = 7,
    Walking = 8,
    Crouching = 9,
    BasicAttacking = 10,
    Ability1ing = 11,
    Ability2ing = 12,
    Ultimating = 13,
};

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

    public bool Hooking;
    public Vector3 HookPoint;

    public enumState CurrentMoveState;
    public enumState CurrentAttackState;
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

