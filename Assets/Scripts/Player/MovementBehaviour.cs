using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody playerRigidBody;
    public void Move(Vector2 inputVec, float speed=1)
    {
        Vector3 velocityVector = new Vector3(inputVec.x, 0f, inputVec.y) * speed;
        playerRigidBody.linearVelocity = new Vector3(velocityVector.x, playerRigidBody.linearVelocity.y, velocityVector.z);
    }
    public void Jump(float jumpingForce)
    {
        playerRigidBody.AddForce(Vector3.up * jumpingForce);
    }
}
