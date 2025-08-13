using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetSpin : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private List<Rigidbody> targetBodies;

    public float amount = 0.1566f;  // docs/img/centrifugal-force-velocity.PNG

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.angularVelocity = Vector3.right * amount;

        if (targetBodies != null)
        {
            for (int i = 0; i < targetBodies.Count; i++)
            {
                Rigidbody body = targetBodies[i];
                float r = (transform.position - body.position).magnitude;
                float x = amount * 138f;
                body.linearVelocity = body.transform.forward * x;
                Debug.Log($"X: {x}");
            }
        }        
    }
    
}
