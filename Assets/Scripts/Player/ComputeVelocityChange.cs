using System.Drawing;
using UnityEngine;

public class ComputeVelocityChange : MonoBehaviour
{
    public void UpdateMovingForce(PlayerController player, Vector2 inputDirection, float maxSpeed, float acceleration, AnimationCurve accelerationFromDot,
        float maxAccelerationForce, AnimationCurve maxAccelerationForceFromDot)
    {
        Vector3 unitInputDirection = GetUnitInputWorld(player, inputDirection);

        Vector3 groundVel = GetGroundVelocity(player);

        (Vector3 m_GoalVel, float velDot) = GetNewGoalVelocity(player, unitInputDirection, groundVel, maxSpeed, acceleration, accelerationFromDot);

        ComputeAndAddActualForce(player, m_GoalVel, maxAccelerationForce, maxAccelerationForceFromDot, velDot);
    }
    public (Vector3, float) GetNewGoalVelocity(PlayerController player, Vector3 unitInputDir, Vector3 groundVel, float maxSpeed, float acceleration, AnimationCurve accelerationFromDot)
    {
        Vector3 m_GoalVel = player._rigidbody.linearVelocity;
        Vector3 unitVel = m_GoalVel.normalized;

        float velDot = Vector3.Dot(m_GoalVel, unitVel);

        float accel = acceleration * accelerationFromDot.Evaluate(velDot);

        Vector3 goalVel = unitInputDir * maxSpeed;

        m_GoalVel = Vector3.MoveTowards(m_GoalVel, goalVel + groundVel, accel * Time.fixedDeltaTime);
        return (m_GoalVel, velDot);
    }
    public void ComputeAndAddActualForce(PlayerController player, Vector3 m_GoalVel, float maxAccelerationForce, AnimationCurve maxAccelerationForceFromDot, float velDot)
    {
        Vector3 neededAccel = (m_GoalVel - player._rigidbody.linearVelocity) / Time.fixedDeltaTime;

        float maxAccel = maxAccelerationForce * maxAccelerationForceFromDot.Evaluate(velDot);

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        player.MovementBehaviour.AddForce(neededAccel * player._rigidbody.mass);
    }
    private Vector3 GetUnitInputWorld(PlayerController player, Vector2 inputDirection) 
    {
        Vector3 groundNormal = player.Raycasts.rayCastHitDown.normal.normalized;

        Vector3 inputForward = player._rigidbody.transform.forward * inputDirection.y;
        Vector3 inputRight = player._rigidbody.transform.right * inputDirection.x;
        Vector3 inputWorld = inputForward + inputRight;
        
        Vector3 inputOnPlane = MathUtils.ProjectOnPlane(inputWorld, groundNormal);
        return inputOnPlane;
    }
    private Vector3 GetGroundVelocity(PlayerController player) 
    {
        RaycastHit rayHit = player.Raycasts.rayCastHitDown;
        Vector3 hitPoint = rayHit.point;
        Collider ground = rayHit.collider;
        if (ground.TryGetComponent<Rigidbody>(out Rigidbody groundRigidBody))
        {
            Vector3 velocity = groundRigidBody.GetPointVelocity(hitPoint);
            //Debug.Log($"hit object has {velocity} velocity");
            return velocity;
        }
        //Debug.Log("hit object has no rigidbody");
        return Vector3.zero;
    }
}
