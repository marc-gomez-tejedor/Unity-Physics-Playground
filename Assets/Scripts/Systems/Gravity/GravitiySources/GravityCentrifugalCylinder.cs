using UnityEngine;

public class GravityCentrifugalCylinder : GravitySource
{
    /// <summary>
    /// fc = V^2 * m / r
    /// Ac = V^2 / r
    /// v = w (angular velocity) * r
    /// Ac = w^2 * r
    /// we will assume that the player is rotating as the same speed as the spaceshop
    /// so lets set w^2 through a constant
    /// </summary>

    [SerializeField]
    float angularVelocity = 0.1566f;
    float angularVelocitySquared;

    [SerializeField, Min(0f)]
    float outerFallOffRadius = 10f;

    void OnValidate()
    {
        gravityType = GravityType.GravityCentrifugalCylinder;

        angularVelocitySquared = angularVelocity * angularVelocity;
    }
    void Awake()
    {
        OnValidate();
    }
    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 rVector = MathUtils.ProjectVectorOnContactPlane(
            position-transform.position, transform.right);
        float r = rVector.magnitude;
        Debug.Log($"pos {transform.position}, bod:{position}, res:{position - transform.position}, rVec:{rVector}, r:{rVector.normalized}");
        Debug.DrawLine(transform.position, transform.position + rVector, Color.magenta);
        return rVector.normalized * angularVelocitySquared * r;
        
    }
    void OnDrawGizmos()
    {
        Vector3 p = transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(p, outerFallOffRadius);
    }    
}
