using UnityEngine;

public static class MathUtils
{
    public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 normal)
    {
        return (vector - normal * Vector3.Dot(vector, normal)).normalized;
    }
    public static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Deg2Rad;
        return direction.x < 0f ? 360f - angle : angle;
    }
}
