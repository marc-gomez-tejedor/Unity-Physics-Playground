using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    [Header("Manual")]
    public float w = 1.0f;
    public float i = 0.0f;
    public float j = 0.0f;
    public float k = 0.0f;
    public Quaternion QuatRotation;
    public Vector3 manualcross = Vector3.zero;
    public float manualangle = 0.0f;
    public bool pressed = false;
    [Header("AutoCorrection")]
    public float wc = 1.0f;
    public float ic = 0.0f;
    public float jc = 0.0f;
    public float kc = 0.0f;
    public Quaternion QuatCorrection;
    public Vector3 crss = new(0.0f, 0.0f, 0.0f);
    public float anglec;    
    public bool pressed2 = false;
    [Header("Parameters")]
    public bool running = true;
    public float rectifyingForce = 1f;
    public float rectifyingSpeedForce = 1f;
    public float torqueImpulseMax = 0.6f;
    public Vector3 appliedForce = Vector3.zero;
    public Vector3 lastDir = Vector3.zero;
    public float speed;
    public float speed2;

    
    public bool onCollision = false;
    public bool testing = true;
    public bool test2 = true;
    public bool test3 = true;

    [Header("References")]
    public Transform targetObject;
    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    private (Vector3,Vector3) pointImpulse;
    private Vector3 collisionTorque;

    private void Start()
    {
        QuatRotation = new(i, j, k, w);
        transform.rotation = QuatRotation;
    }
    public void Center()
    {
        if (!running) { return; }
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
        //if (!onCollision) { return; }
        if (test3) { RotateQuaternion(); return; }
        if (test2)
        {
            Test2();
            desiredOrientation = -Test2();
            TryTorques();
            return;
        }
        else desiredOrientation = -targetObject.up;
                
        float d = Vector3.Dot(transform.up, desiredOrientation);

        Quaternion desRot;
        if (d > 0.99999f) desRot = Quaternion.identity;
        else desRot = Quaternion.FromToRotation(transform.up, desiredOrientation);

        transform.rotation = desRot * transform.rotation;
        Debug.Log($"quaternion {desRot}");
    }

    Vector3 Test2()
    {
        Vector3 g = Vector3.down; //asumming simple gravity
        Vector3 normal = -targetObject.up; //plane normal

        float beta = GetBeta(normal, g);

        Vector3 desiredDir = Vector3.RotateTowards(g, normal, -beta, 1);
        Quaternion des = Quaternion.identity;
        return desiredDir;
    }
    float GetBeta(Vector3 normal, Vector3 g)
    {
        float r = 0.5f * 1.5f;
        float d = 1f * 1.5f;
        float alpha = Vector3.Angle(normal, g);
        float h = r * Mathf.Sin(alpha);
        float sinBeta = h / d;
        float beta = Mathf.Asin(sinBeta);
        return beta;
    }
    void TryTorques()
    {
        Vector3 direction = (desiredOrientation - transform.up);
        float dst  = direction.magnitude;
        direction = direction.normalized;
        Vector3 angVel = _rigidbody.angularVelocity;
        Debug.Log(angVel);
        Vector3 forceChange = (direction - appliedForce)* dst;
        appliedForce += forceChange;
        Vector3 rateChange = direction - lastDir;
        lastDir = direction;
        float lastDst = dst;
        _rigidbody.AddForceAtPosition(forceChange * speed + rateChange * speed2, transform.position+transform.up*transform.localScale.y);
        _rigidbody.AddForce(forceChange * speed + rateChange * speed2);
    }

    void UpdateManualParameters()
    {
        manualangle = Mathf.Acos(w);
        manualcross = new Vector3(i, j, k)/Mathf.Sin(manualangle);

        QuatRotation = new(i, j, k, w);
    }
    void UpdateAutoCorrectionParameters()
    {
        crss = Vector3.Cross(desiredOrientation, transform.up);
        crss = crss.normalized;
        anglec = Vector3.Angle(transform.up, desiredOrientation) / 2;
        anglec *= Mathf.Deg2Rad;
        crss *= Mathf.Sin(anglec);
        wc = Mathf.Cos(anglec);
        ic = crss.x;
        jc = crss.y;
        kc = crss.z;
        QuatCorrection = new(ic, jc, kc, wc);
    }
    void RotateQuaternion()
    {
        UpdateManualParameters();
        UpdateAutoCorrectionParameters();
        if (pressed)
        {
            pressed = false;
            transform.rotation *= QuatRotation;
        }
        if (pressed2)
        {
            pressed2 = false;
            transform.rotation *= QuatCorrection;
        }
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
        if (!running) { return; }
        Debug.Log($"collision enter: {collision.impulse}");
        CompensateCollisionTorques(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        onCollision = true;
        if (!running) { return; }
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
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 3f);
        /*Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);*/
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + crss * 3f);
    }
}
