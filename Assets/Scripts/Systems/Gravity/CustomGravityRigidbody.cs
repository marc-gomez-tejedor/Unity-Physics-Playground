using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour
{
    Rigidbody body;

    float floatDelay;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    void FixedUpdate()
    {
        if (body.IsSleeping())
        {
            floatDelay = 0f;
            return;
        }
        if (body.linearVelocity.sqrMagnitude < 0.0001f)
        {
            floatDelay += Time.deltaTime;
            if (floatDelay > 1f)
            {
                return;
            }
        }
        else
        {
            floatDelay = 0f;
        }
        body.AddForce(CustomGravity.GetGravity(body.position), ForceMode.Acceleration);    
    }
}
