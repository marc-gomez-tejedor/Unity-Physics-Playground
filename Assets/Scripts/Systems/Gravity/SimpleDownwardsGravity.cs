using Unity.VisualScripting;
using UnityEngine;

public class SimpleDownwardsGravity : MonoBehaviour, IInitializable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float gravity = 9.81f;

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
        _rigidbody.AddForce(Vector3.down * _rigidbody.mass * gravity);
    }
}
