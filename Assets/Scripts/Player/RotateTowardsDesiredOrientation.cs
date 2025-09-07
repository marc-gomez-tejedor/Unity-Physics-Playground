using UnityEngine;

public class RotateTowardsDesiredOrientation : MonoBehaviour
{
    private Quaternion QuatCorrection;

    [Header("References")]
    public PlayerController playerController;
    public Vector3 desiredOrientation { get; private set; } = Vector3.up;  

    public Quaternion GetQuaternion(Rigidbody _rigidbody)
    {
        UpdateDesiredDirection();
        UpdateAutoCorrectionParameters(_rigidbody);
        return QuatCorrection;
    }

    public Quaternion GetQuaternion(Rigidbody _rigidbody, Vector3 direction)
    {
        desiredOrientation = direction;
        UpdateAutoCorrectionParameters(_rigidbody);
        return QuatCorrection;
    }
    private void UpdateAutoCorrectionParameters(Rigidbody _rigidbody)
    {
        QuatCorrection = Quaternion.FromToRotation(_rigidbody.transform.up, desiredOrientation);
    }
    private void UpdateDesiredDirection()
    {
        Vector3 f;
        if (playerController.currentFieldForce == Vector3.zero)
        {
            desiredOrientation = Vector3.up;
            return;
        }
        f = playerController.currentFieldForce;
        desiredOrientation = -f;
    }
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + desiredOrientation * 10f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 3f);
    }
}
