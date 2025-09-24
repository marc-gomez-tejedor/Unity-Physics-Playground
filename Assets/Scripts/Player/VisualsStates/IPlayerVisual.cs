using UnityEngine;

public struct PlayerVisualContext
{
    public Vector3 velocity;
    public Vector3 upAxis;
    public bool Climbing, Swimming, OnGround, OnSteep;
    public Rigidbody connectedBody, previousConnectedBody;
}

public interface IPlayerVisual
{
    public void UpdateVisuals(PlayerVisualContext ctx)
    {

    }
}
