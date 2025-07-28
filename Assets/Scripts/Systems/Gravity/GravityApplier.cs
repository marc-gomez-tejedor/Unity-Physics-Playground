using UnityEngine;

public class GravityApplier : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float gravity = 120f;

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Vector3.down * _rigidbody.mass * gravity);
    }
}
