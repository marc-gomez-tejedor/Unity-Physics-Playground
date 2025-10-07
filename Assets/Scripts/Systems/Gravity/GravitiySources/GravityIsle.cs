using UnityEngine;
using UnityEngine.ProBuilder;

public class GravityIsle : GravitySource
{
    [SerializeField]
    float gravity = 9.81f; //positive pulls, negative repels

    [SerializeField]
    Transform pullCenter;

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
        float upVec = Vector3.Dot(-dir, -vector);
        if (upVec < 0f || distance > outerFalloffRadius)
        {
            Debug.Log($"zero: t:{transform.position}, v:{vector}, u:{upVec}, d:{distance}");
            return Vector3.zero;
        }
        float g = gravity;
        if (distance > outerRadius)
        {
            g *= 1f - (distance - outerRadius) * outerFalloffFactor;
        }
        Debug.Log($"one: g:{g} dir{dir} t:{transform.position}, v:{vector}, u:{upVec}, d:{distance}");
        return g * dir;
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
