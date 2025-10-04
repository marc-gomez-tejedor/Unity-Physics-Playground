using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetSpin : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;

    public float amount = 0.1566f;  // docs/img/sketeches/centrifugal-force-velocity.PNG

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.angularVelocity = Vector3.right * amount;
    }
    
}
