using UnityEngine;

public static class MathUtils
{
    public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 normal)
    {
        return vector - normal * Vector3.Dot(vector, normal);
    }
}
