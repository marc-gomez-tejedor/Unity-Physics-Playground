using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rigidBody;

    public void Move(Vector2 inputVec, float speed=1)
    {
        Vector3 velocityVector = new Vector3(inputVec.x, 0f, inputVec.y) * speed;
        _rigidBody.linearVelocity = new Vector3(velocityVector.x, _rigidBody.linearVelocity.y, velocityVector.z);
    }
    public void Jump(float jumpingForce)
    {
        _rigidBody.AddForce(Vector3.up * jumpingForce);
    }
}
