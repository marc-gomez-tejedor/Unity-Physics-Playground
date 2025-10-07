using UnityEngine;
using UnityEngine.ProBuilder;

public class GravityIsle : GravitySource
{
    [SerializeField]
    float gravity = 9.81f; //positive pulls, negative repels

    [SerializeField, Min(0f)]
    float outerRadius = 10f, outerFalloffRadius = 15f;

    float outerFalloffFactor;

    void OnValidate()
    {
        outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);

        outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
    }
    void Awake()
    {
        OnValidate();
    }
    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 dir = -transform.up;
        Vector3 vector = transform.position - position;
        float distance = vector.magnitude;
        float upVec = Vector3.Dot(-dir, vector);
        if (upVec > 0f || distance > outerFalloffRadius)
        {
            return Vector3.zero;
        }
        if (distance > outerRadius)
        {
            gravity *= 1f - (distance - outerRadius) * outerFalloffFactor;
            dir = vector.normalized;
        }
        return gravity * dir;
    }
    void OnDrawGizmos()
    {
        Vector3 p = transform.position;
        Gizmos.DrawWireSphere(p, outerRadius);
        if (outerFalloffRadius > outerRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, outerFalloffRadius);
        }
    }
}
