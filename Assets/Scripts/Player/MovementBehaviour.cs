using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rigidBody;

    [Header("Parameters")]
    public Vector3 appliedForce = Vector3.zero;

    public void Move(Vector2 inputVec, float speed=1)
    {
        Vector3 velocityVector = new Vector3(inputVec.x, 0f, inputVec.y) * speed;
        _rigidBody.linearVelocity = new Vector3(velocityVector.x, _rigidBody.linearVelocity.y, velocityVector.z);
    }

    public void AddSpeed(Vector2 inputVec, float speed=1)
    {
        Vector3 inputX = _rigidBody.transform.right * inputVec.x;
        Vector3 inputY = _rigidBody.transform.forward * inputVec.y;
        Vector3 worldInput3D = inputX + inputY;
        Vector3 forceChange = worldInput3D - appliedForce;
        appliedForce += forceChange;

        /*
        Vector3 inputX = _rigidBody.transform.right * forceChange.x;
        Vector3 inputY = _rigidBody.transform.forward * forceChange.y;
        Vector3 input3D = inputX + inputY;
        */
        _rigidBody.AddForce(forceChange * speed);
    }
    public void Jump(float jumpingForce)
    {
        _rigidBody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }
}
