using UnityEngine;

public static class MathUtils
{
    public static Vector3 ProjectDirectionOnContactPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }
    public static Vector3 ProjectVectorOnContactPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal));
    }
    public static float GetAngle(Vector2 direction)
    {
        float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
        return direction.x < 0f ? 360f - angle : angle;
    }
}
