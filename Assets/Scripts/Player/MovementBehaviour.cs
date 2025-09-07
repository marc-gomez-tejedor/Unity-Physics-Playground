using UnityEditor;
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
    public void Move3D(Vector2 inputVec, float speed=1)
    {
        Vector3 inputX = _rigidBody.transform.right * inputVec.x;
        Vector3 inputY = _rigidBody.transform.forward * inputVec.y;
        Vector3 worldInput3D = inputX + inputY;
        //_rigidBody.lin move inputs referenced to camera not capsule
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

    public void UpdateFloatingSpringPosition(PlayerController player, 
        float RideHeight, float RideSpringStrength, float RideSpringDamper)
    {
        bool _rayDidHit = player.Raycasts.didRaycastHitDown;
        Vector3 DownDir = player.Raycasts.DownDir;

        Rigidbody _RB = player._rigidbody;

        if (_rayDidHit)
        {
            RaycastHit _rayHit = player.Raycasts.rayCastHitDown;
            Vector3 vel = _RB.linearVelocity;
            Vector3 rayDir = DownDir; //this should be =to forcefield
            
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

            //Debug.DrawLine(_RB.transform.position, _RB.transform.position + (rayDir * springForce), Color.yellow);

            player._rigidbody.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }
    }
    
    public void UpdateUprightForce(PlayerController player, float strength, float damper)
    {
        Quaternion characterCurrent = _rigidBody.transform.rotation;
        Quaternion toGoal = player.Orientate.GetQuaternion(player._rigidbody);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        _rigidBody.AddTorque((rotAxis * (rotRadians * strength)) - (_rigidBody.angularVelocity * damper));
    }
    
    public void AddForce(Vector3 force)
    {
        _rigidBody.AddForce(force);
    }
    
    public void Jump(float jumpingForce)
    {
        _rigidBody.AddForce(Vector3.up * jumpingForce, ForceMode.Impulse);
    }
}
