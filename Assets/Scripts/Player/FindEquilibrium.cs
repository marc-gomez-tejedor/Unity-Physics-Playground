using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    [Header("Parameters")]
    public float rectifyingForce = 1f;

    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force */

    public Rigidbody _rigidbody;
    private List<(Vector3, Vector3, Vector3)> pointImpulses = new List<(Vector3, Vector3, Vector3)>();
    private List<Vector3> collisionTorques = new List<Vector3>();

    public void Center()
    {
        // check floor with playercontroller.raycasts (for now will be vector.up)
        /*DebugPointImpulses();
        ComputeCollisionTorques();
        DebugTorques();
        AddTorques();*/
    }
    void AddTorques()
    {
        float xSum = 0;
        float ySum = 0;
        float zSum = 0;

        for (int i = 0; i < collisionTorques.Count; i++)
        {
            xSum += collisionTorques[i].x;
            ySum += collisionTorques[i].y;
            zSum += collisionTorques[i].z;
        }
        Vector3 result = new Vector3(-xSum, -ySum, -zSum) * rectifyingForce;
        // lerp eventually WIP
        _rigidbody.AddTorque(result / Time.fixedDeltaTime);
    }

    void ComputeCollisionTorques()
    {
        collisionTorques = new List<Vector3>();
        Matrix4x4 localToWoldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        foreach (var pointImpulse in pointImpulses)
        {
            // transpose world coordinates to local coords
            Vector3 worldContactPoint = pointImpulse.Item1;
            Vector3 worldImpulse = pointImpulse.Item2;

            Vector3 localContactPoint = localToWoldMatrix.inverse.MultiplyPoint3x4(worldContactPoint);
            Vector3 localImpulse = localToWoldMatrix.inverse.MultiplyPoint3x4(worldImpulse);

            Vector3 r = transform.position - localContactPoint;
            Vector3 f = localImpulse;
            Vector3 t = Vector3.Cross(r,f);

            collisionTorques.Add(t);
        }
    }
    void UpdatePointImpulses(Collision collision)
    {
        int i = 0;
        pointImpulses = new List<(Vector3, Vector3, Vector3)>();
        foreach (ContactPoint contact in collision.contacts)
        {
            i++;
            Vector3 point = contact.point;
            Vector3 impulse = contact.impulse;
            Vector3 normal = contact.normal;
            pointImpulses.Add((point, impulse, normal));
        }
        ComputeCollisionTorques();
        AddTorques();
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision enter: {collision}");
        UpdatePointImpulses(collision);
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"collision stay: {collision}");
        UpdatePointImpulses(collision);
    }
    void DebugPointImpulses()
    {
        for (int i = 0; i < pointImpulses.Count; i++)
        {
            Debug.Log($"Element n{i}, point:{pointImpulses[i].Item1}, impulse:{pointImpulses[i].Item2}, normal:{pointImpulses[i].Item3}");
        }
    }
    void DebugTorques()
    {
        for (int i = 0; i < collisionTorques.Count; i++)
        {
            Debug.Log($"Element n{i}, torque:{collisionTorques[i]}");
        }
    }

    // For debugging
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + desiredOrientation*3f);
    }
}
