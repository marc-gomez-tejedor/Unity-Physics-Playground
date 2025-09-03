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
    public bool onCollision = false;

    [Header("References")]
    public PlayerController playerController;
    public Transform targetObject = null;
    private Vector3 desiredOrientation = Vector3.up;  /* normal unit vector (default up for testing)
    * lerp this orientation with the current one based on rectifyingForce
    * apply that to the rectifying torques to compensate external ones multiplied by the biased lerp force (done-sort of)*/

    public Rigidbody _rigidbody;
    public void Center()
    {
        if (!running) { return; }
        UpdateAutoCorrectionParameters();
        if (onCollision) { Rotate(); }
    }
    private void Rotate()
    {
        UpdateDesiredDirection();
        UpdateAutoCorrectionParameters();
        playerController._rigidbody.angularVelocity = Vector3.zero;
        transform.rotation *= QuatCorrection;
    }

    private void UpdateAutoCorrectionParameters()
    {
        crss = Vector3.Cross(desiredOrientation, transform.up);
        crss = crss.normalized;
        anglec = Vector3.Angle(transform.up, desiredOrientation) / 2;
        anglec *= Mathf.Deg2Rad;
        crss = new Vector3(crss.x, crss.y, crss.z);

        Quaternion undo = Quaternion.Inverse(transform.rotation);
        localAutoCross = (undo * crss).normalized;
        localAutoCross *= Mathf.Sin(anglec);
        wc = Mathf.Cos(anglec);
        ic = localAutoCross.x;
        jc = localAutoCross.y;
        kc = localAutoCross.z;
        QuatCorrection = new(-ic, -jc, -kc, wc);
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
        Vector3 normal = -targetObject.up.normalized; //plane normal
        Debug.Log($"normal {normal}");

        float beta = GetBeta(normal, f);
        Debug.Log($"beta {beta}");
        desiredOrientation = Vector3.RotateTowards(-f, normal, -beta, 1);
        Debug.Log($"deired orien {desiredOrientation}");
    }
    private float GetBeta(Vector3 normal, Vector3 f)
    {
        float dot = Vector3.Dot(normal, -f);
        float alpha = 0;
        if (dot > 0.999f) return 0;
        float r = 0.5f * 1.5f; //sphere radius (sphere of the base of the capsule)
        float d = 1f * 1.5f; //distance from center of the sphere to the centeer of the capsule
        alpha = Vector3.Angle(normal, -f) * Mathf.Deg2Rad;
        float h = r * Mathf.Sin(alpha);
        float sinBeta = h / d;
        float beta = Mathf.Asin(sinBeta);
        return beta;
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollision = true;
        targetObject = collision.gameObject.transform;
    }
    private void OnCollisionStay(Collision collision)
    {
        onCollision = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        onCollision = false;
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
