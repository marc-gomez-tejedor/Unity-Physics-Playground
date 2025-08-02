using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetSpin : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private List<Rigidbody> targetBodies;

    public float amount = 5f;

    public void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddTorque(Vector3.right * amount, ForceMode.Impulse);
    }
    
}
