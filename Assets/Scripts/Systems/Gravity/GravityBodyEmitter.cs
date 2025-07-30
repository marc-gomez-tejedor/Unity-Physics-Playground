using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityBodyEmitter : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;

    private List<Rigidbody> allOtherMassBodies = new List<Rigidbody>();
    private int otherBodiesCount;

    public bool pulling = false;

    public void Initialize()
    {
        List<Rigidbody> allMassBodies = Game.Gravity.GetMassBodies();
        for (int i = 0; i < allMassBodies.Count; i++)
        {
            Rigidbody body = allMassBodies[i];
            if (body != _rigidbody)
            {
                allOtherMassBodies.Add(body);
            }
        }
        otherBodiesCount = allOtherMassBodies.Count;
    }

    private void FixedUpdate()
    {
        if (pulling)
        {
            for (int i = 0; i < otherBodiesCount; i++)
            {
                Rigidbody bodyToApply = allOtherMassBodies[i];
                Vector3 forceToApply = GetGravityForce(bodyToApply);
                Debug.Log($"this: {_rigidbody} is applying: (x{forceToApply.x}, y{forceToApply.y}, z{forceToApply.z}) to {bodyToApply}");
                bodyToApply.AddForce(forceToApply, ForceMode.Force);
            }
        }
    }

    private Vector3 GetGravityForce(Rigidbody targetBody)
    {
        // Newton's Universal Gravitational Law:
        //       F = -G(m1*m2)/(r^2) * ur     

        Vector3 r = targetBody.transform.position - _rigidbody.transform.position;
        Vector3 ur = r.normalized;

        double G = AllGravityBodies.NEWTONS_GRAVITATIONAL_CONSTANT;
        double m1 = _rigidbody.mass;
        double m2 = targetBody.mass;

        double numerator = (double)(G * m1 * m2);
        double denominator = (double)(r.magnitude * r.magnitude);
        double result = numerator / denominator;

        return ur * (float)-result;
    }
}
