using Unity.VisualScripting;
using UnityEngine;

public class SimpleForceApplier : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;
    public float magnitude = 9.81f;
    public Vector3 force = Vector3.down;

    public void Initialize()
    {
        if (this.isActiveAndEnabled)
        {
            _rigidbody.useGravity = false;
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        force = force.normalized;
        _rigidbody.AddForce(force * _rigidbody.mass * magnitude);
    }
}
