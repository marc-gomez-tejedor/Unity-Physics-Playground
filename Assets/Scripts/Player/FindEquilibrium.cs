using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    [Header("Parameters")]
    public float rectifyingForce = 0.8756839f;

    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    private List<(Vector3, Vector3, Vector3)> pointImpulses = new List<(Vector3, Vector3, Vector3)>();
    private List<Vector3> collisionTorques = new List<Vector3>();

    public void Center()
    {
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
        DebugPointImpulses();
        ComputeCollisionTorquesDebugged();
        DebugTorques();
        AddTorques();
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
        Debug.Log($"res {result}");
        _rigidbody.AddRelativeTorque(result * rectifyingForce, ForceMode.Impulse);
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
    void ComputeCollisionTorquesDebugged()
    {
        int i = 0;
        collisionTorques = new List<Vector3>();
        
        Debug.Log($"--TRANSPOSING WORLD TO LOCAL AND THEN COMPUTING TORQUES--");
        foreach (var pointImpulse in pointImpulses)
        {
            Vector3 r = _rigidbody.worldCenterOfMass - pointImpulse.Item1;
            Vector3 f = pointImpulse.Item2;
            Vector3 t = Vector3.Cross(r, f);

            Debug.Log($"Element n{i}, r:{r}, f:{f}, t:{t}");

            collisionTorques.Add(t);
            i++;
        }
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
        Debug.Log($"--DEBUGGING POINT IMPULSES--");
        for (int i = 0; i < pointImpulses.Count; i++)
        {
            Debug.Log($"Element n{i}, point:{pointImpulses[i].Item1}, impulse:{pointImpulses[i].Item2}, normal:{pointImpulses[i].Item3}");
        }
    }
    void DebugTorques()
    {
        Debug.Log($"--DEBUGGING TORQUES--");
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
