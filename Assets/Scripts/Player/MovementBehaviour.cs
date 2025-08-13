using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rigidBody;

    [Header("Parameters")]
    public Vector2 appliedForce = Vector2.zero;

    public void Move(Vector2 inputVec, float speed=1)
    {
        Vector3 velocityVector = new Vector3(inputVec.x, 0f, inputVec.y) * speed;
        _rigidBody.linearVelocity = new Vector3(velocityVector.x, _rigidBody.linearVelocity.y, velocityVector.z);
    }

    public void AddSpeed(Vector2 inputVec, float speed=1)
    {
        float forceChangeX = inputVec.x - appliedForce.x;
        float forceChangeY = inputVec.y - appliedForce.y;
        appliedForce.x += forceChangeX;
        appliedForce.y += forceChangeY;

        Vector3 inputX = _rigidBody.transform.right * forceChangeX;
        Vector3 inputY = _rigidBody.transform.forward * forceChangeY;
        Vector3 input3D = inputX + inputY;

        _rigidBody.linearVelocity += input3D * speed;
    }
    public void Jump(float jumpingForce)
    {
        _rigidBody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }
}
