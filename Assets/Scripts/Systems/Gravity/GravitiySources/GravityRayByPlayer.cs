using UnityEngine;

public class GravityRayByPlayer : GravitySource
{
    [SerializeField]
    float gravity = 9.81f; //positive pulls, negative repels

    [SerializeField, Min(0f)]
    float maxDistance = 20f;
    public Vector3 hitPosition;

    public override bool RequireExplicitInclude => true;
    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 vec = hitPosition - position;
        float distance = vec.magnitude;
        return vec.normalized * gravity /** distance/maxDistance*/;
    }
}
