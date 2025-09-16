using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float speed = 10f;

    void Accelerate(Rigidbody body)
    {
        Vector3 velocity = body.linearVelocity;
        if (velocity.y >= speed)
        {
            return;
        }

        velocity.y = speed;
        body.linearVelocity = velocity;
    }
    void OnTriggerEnter(Collider other)
    {
        Rigidbody body = other.attachedRigidbody;
        if (body)
        {
            Accelerate(body);
            if (body.TryGetComponent(out MovingSphere sphere))
            {
                sphere.PreventSnapToGround();
            }
        }
    }

}
