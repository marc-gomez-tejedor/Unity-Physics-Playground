using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class StartOrbitalVelocity : MonoBehaviour, IInitializable
{
    [SerializeField] GameObject starObjcet;
    private Rigidbody _rigidbody;
    public Vector3 initialVelocity;

    public void Initialize()
    {
        // Orbital velocity based on Fc (centripetal force) = Fg (gravitational force):
        //                  v = sqrt((G*M) / (r))

        _rigidbody = GetComponent<Rigidbody>();
        Rigidbody starRigidbody = starObjcet.GetComponent<Rigidbody>();
        StartOrbitalVelocity star = starObjcet.GetComponent<StartOrbitalVelocity>();

        float G = AllGravityBodies.NEWTONS_GRAVITATIONAL_CONSTANT;
        float M = starRigidbody.mass;
        
        Vector3 rVector = starRigidbody.position - transform.position;
        float rMagnitude = rVector.magnitude;

        double numerator = (double)(G * M);
        double resPreRoot = (double)(numerator/rMagnitude);

        double velocity = Math.Sqrt(resPreRoot);

        Vector3 resUnitariVec = math.cross(rVector.normalized, Vector3.up);
        Vector3 oribatalVelocityVector = resUnitariVec.normalized * (float)velocity;

        initialVelocity = oribatalVelocityVector;
        if (star != null) initialVelocity += star.initialVelocity;

        _rigidbody.AddForce(initialVelocity, ForceMode.VelocityChange);
        Debug.Log($"{_rigidbody}: {initialVelocity.magnitude}");
    }
}
 