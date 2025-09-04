using UnityEngine;

public class RotateTowardsDesiredOrientation : MonoBehaviour
{
    [Header("AutoCorrection")]
    private float wc = 1.0f;
    private float ic = 0.0f;
    private float jc = 0.0f;
    private float kc = 0.0f;
    private Quaternion QuatCorrection;
    private Vector3 crss = Vector3.zero;
    private Vector3 localAutoCross = Vector3.zero;
    private float anglec;
    [Header("Parameters")]
    public bool running = true;

    [Header("References")]
    public PlayerController playerController;
    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    public Quaternion GetQuaternion()
    {
        UpdateDesiredDirection();
        UpdateAutoCorrectionParameters();
        return QuatCorrection;
    }

    public Quaternion GetQuaternion(Vector3 direction)
    {
        UpdateDesiredDirection(direction);
        UpdateAutoCorrectionParameters();
        return QuatCorrection;
    }
    private void UpdateAutoCorrectionParameters()
    {
        _rigidbody = playerController._rigidbody;
        //QuatCorrection = Quaternion.LookRotation(playerController.transform.forward, desiredOrientation);
        QuatCorrection = Quaternion.FromToRotation(_rigidbody.transform.up, desiredOrientation);
        /*
        crss = Vector3.Cross(desiredOrientation, _rigidbody.transform.up);
        crss = crss.normalized;
        anglec = Vector3.Angle(_rigidbody.transform.up, desiredOrientation) / 2;
        anglec *= Mathf.Deg2Rad;

        Quaternion undo = Quaternion.Inverse(_rigidbody.transform.rotation);
        localAutoCross = (undo * crss).normalized;
        localAutoCross *= Mathf.Sin(anglec);
        wc = Mathf.Cos(anglec);
        ic = localAutoCross.x;
        jc = localAutoCross.y;
        kc = localAutoCross.z;
        QuatCorrection = new(-ic, -jc, -kc, wc);   */
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
    private void UpdateDesiredDirection(Vector3 direction)
    {
        desiredOrientation = direction;
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
