using UnityEngine;

public class GravitySphere : GravitySource
{
    [SerializeField]
    float gravity = 9.81f;

    [SerializeField, Min(0f)]
    float outerRadius = 10f, outerFalloffRadius = 15f;

    void OnValidate()
    {
        outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);    
    }
    void Awake()
    {
        OnValidate();
    }
    void OnDrawGizmos()
    {
        Vector3 p = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(p, outerRadius);
        if (outerFalloffRadius > outerRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere (p, outerFalloffRadius);
        }
    }
}
