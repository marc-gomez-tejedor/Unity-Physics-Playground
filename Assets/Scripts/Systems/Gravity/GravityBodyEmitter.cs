using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityBodyEmitter : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;

    private List<Rigidbody> allOtherMassBodies = new List<Rigidbody>();
    private int otherBodiesCount;

    public void Initialize()
    {
        List<Rigidbody> allMassBodies = Game.Gravity.GetMassBodies();
        for (int i = 0; i < allMassBodies.Count; i++)
        {
            Rigidbody body = allMassBodies[i];
            Debug.Log($"self is {this.gameObject} and {body}");
            if (body != _rigidbody)
            {
                Debug.Log($"self is {this.gameObject} and {body} passed check");
                allOtherMassBodies.Add(body);
            }
        }
        otherBodiesCount = allOtherMassBodies.Count;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < otherBodiesCount; i++)
        {
            Rigidbody bodyToApply = allOtherMassBodies[i];
            _rigidbody.AddForce(GetGravityForce(bodyToApply));
        }
    }

    private Vector3 GetGravityForce(Rigidbody targetBody)
    {
        // Newton's Universal Gravitational Law:
        //       F = -G(m1*m2)/(r^2) * ur     

        Vector3 r = targetBody.transform.position - _rigidbody.transform.position;
        Vector3 ur = r.normalized;

        float G = AllGravityBodies.NEWTONS_GRAVITATIONAL_CONSTANT;
        float m1 = _rigidbody.mass;
        float m2 = targetBody.mass;

        double numerator = (double)(G * m1 * m2);
        double denominator = (double)(r.magnitude * r.magnitude);
        double result = numerator / denominator;

        return ur * (float)-result;
    }
}
