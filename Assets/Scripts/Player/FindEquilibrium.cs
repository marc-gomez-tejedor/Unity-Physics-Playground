using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    [Header("Parameters")]
    public float rectifyingForce = 1f;
    public float rectifyingSpeedForce = 1f;
    public float torqueImpulseMax = 0.6f;

    private bool onCollision = false;
    public bool testing = true;

    [Header("References")]
    public Transform targetObject;
    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    private (Vector3,Vector3) pointImpulse;
    private Vector3 collisionTorque;

    public void Center()
    {
        if (testing)
        {
            Test();
            return;
        }
        if (onCollision) { Rotate(); }
        ComputeAllForces();
        DebugTorques();
        AddTorques();
    }
    private void Rotate()
    {
        ComputeOffset();
        Vector3 dir = desiredOrientation - transform.up;
        float angle = Vector3.Angle(desiredOrientation, transform.up);
        float l = angle/180f;
        Vector3 r = (_rigidbody.worldCenterOfMass + transform.up * transform.localScale.y) - _rigidbody.worldCenterOfMass;
        Vector3 f = dir * l / Time.fixedDeltaTime;
        Vector3 t = Vector3.Cross(r, f);
        _rigidbody.AddTorque(t * rectifyingSpeedForce, ForceMode.Impulse);
    }

    private void ComputeOffset()
    {
        Vector3 targetAngularVelocity;
        Vector3 selfAngularVelocity = _rigidbody.angularVelocity;
        if (targetObject == null)
        {
            desiredOrientation = targetObject.transform.up; 
            desiredOrientation = desiredOrientation.normalized;

            //targetAngularVelocity = targetObject.angularVelocity;
        }
        else
        {
            // set to up and zero so the player wants to stand upright for debugging and default by now
            desiredOrientation = Vector3.up;
            targetAngularVelocity = Vector3.zero;
        }
        float angle = Vector3.Angle(desiredOrientation, transform.up);
    }

    private void ComputeAllForces()
    {
        Vector3 forces = _rigidbody.GetAccumulatedForce();
        //Debug.Log($"accForces: {forces}");
        Vector3 torques = _rigidbody.GetAccumulatedTorque();
        //Debug.Log($"accTorques: {torques}");
    }
    private void AddTorques()
    {
        Debug.Log($"res {-collisionTorque}");
        _rigidbody.AddTorque(-collisionTorque * rectifyingForce, ForceMode.Force);
    }

    void Test()
    {
        if (!onCollision) { return; }
        desiredOrientation = -targetObject.up;
        
        float d = Vector3.Dot(transform.up, desiredOrientation);

        Quaternion desRot;
        if (d > 0.99999f) desRot = Quaternion.identity;
        else desRot = Quaternion.FromToRotation(transform.up, desiredOrientation);

        Quaternion a = Quaternion.Slerp(Quaternion.identity, desRot, torqueImpulseMax);
        transform.rotation = a * transform.rotation;
        Debug.Log($"quaternion {desRot}");
    }

    //collision torque
    private void CompensateCollisionTorques(Collision collision)
    {
        if (testing)
        {
            Test();
            return;
        }
        UpdatePointImpulse(collision);
        DebugPointImpulse();
        ComputeCollisionTorque();
    }
    private void UpdatePointImpulse(Collision collision)
    {
        int i = 0;
        ContactPoint contact = collision.GetContact(0);
        pointImpulse = (contact.point, contact.impulse);
    }
    private void ComputeCollisionTorque()
    {
        Vector3 r = _rigidbody.worldCenterOfMass - pointImpulse.Item1;
        Vector3 f = pointImpulse.Item2/Time.fixedDeltaTime;
        Vector3 t = Vector3.Cross(r, f);

        Debug.Log($"--TRANSPOSING WORLD TO LOCAL AND THEN COMPUTING TORQUES--");
        Debug.Log($"r:{r}, f:{f}, t:{t}");

        collisionTorque = t;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        onCollision = true;
        targetObject = collision.gameObject.transform;
        Debug.Log($"collision enter: {collision.impulse}");
        CompensateCollisionTorques(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        onCollision = true;
        Debug.Log($"collision stay: {collision.impulse}");
        CompensateCollisionTorques(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        onCollision = false;
    }

    // For debugging
    private void DebugPointImpulse()
    {
        Debug.Log($"--DEBUGGING POINT IMPULSES--");
        Debug.Log($"point:{pointImpulse.Item1}, impulse:{pointImpulse.Item2}");
    }
    private void DebugTorques()
    {
        Debug.Log($"--DEBUGGING TORQUES--");
        Debug.Log($"torque:{collisionTorque}");
    }

    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + desiredOrientation*3f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 3f);
    }
}
