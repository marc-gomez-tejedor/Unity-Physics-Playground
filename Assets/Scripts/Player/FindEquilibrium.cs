using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    [Header("Parameters")]
    public float rectifyingForce = 0.8756839f;

    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    private (Vector3,Vector3) pointImpulse;
    private Vector3 collisionTorque;

    public void Center()
    {
    }
    void CompensateTorques(Collision collision)
    {
        UpdatePointImpulses(collision);
        DebugPointImpulse();
        ComputeCollisionTorque();
        DebugTorque();
        AddTorque();
    }
    void UpdatePointImpulses(Collision collision)
    {
        int i = 0;
        ContactPoint contact = collision.GetContact(0);
        pointImpulse = (contact.point, contact.impulse);
    }
    void AddTorque()
    {
        float x = collisionTorque.x;
        float y = collisionTorque.y;
        float z = collisionTorque.z;

        Vector3 result = new Vector3(-x, -y, -z) * rectifyingForce;
        Debug.Log($"res {result}");
        _rigidbody.AddTorque(result * rectifyingForce, ForceMode.Impulse);
    }

    void ComputeCollisionTorque()
    {
        Vector3 r = _rigidbody.worldCenterOfMass - pointImpulse.Item1;
        Vector3 f = pointImpulse.Item2;
        Vector3 t = Vector3.Cross(r, f);

        Debug.Log($"--TRANSPOSING WORLD TO LOCAL AND THEN COMPUTING TORQUES--");
        Debug.Log($"r:{r}, f:{f}, t:{t}");

        collisionTorque = t;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"collision enter: {collision}");
        CompensateTorques(collision);
    }
    void OnCollisionStay(Collision collision)
    {
        Debug.Log($"collision stay: {collision}");
        CompensateTorques(collision);
    }
    void DebugPointImpulse()
    {
        Debug.Log($"--DEBUGGING POINT IMPULSES--");
        Debug.Log($"point:{pointImpulse.Item1}, impulse:{pointImpulse.Item2}");
    }
    void DebugTorque()
    {
        Debug.Log($"--DEBUGGING TORQUES--");
        Debug.Log($"torque:{collisionTorque}");
    }

    // For debugging
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + desiredOrientation*3f);
    }
}
