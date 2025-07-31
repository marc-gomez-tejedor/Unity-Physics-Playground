using Unity.VisualScripting;
using UnityEngine;

public class SimpleDownwardsGravity : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float gravity = 120f;

    public void Initialize()
    {
        _rigidbody.useGravity = false;
        if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.down * _rigidbody.mass * gravity);
    }
}
