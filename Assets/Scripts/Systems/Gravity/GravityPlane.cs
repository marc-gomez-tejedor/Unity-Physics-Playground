using UnityEngine;

public class GravityPlane : GravitySource
{
    [SerializeField]
    float gravity = 9.81f;

    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 up = transform.up;
        return -gravity * up;
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 0f, 1f));
    }
}
