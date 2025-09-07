using UnityEngine;

public class SimpleMoveState : PlayerState
{
    [Header("General Parameters")]
    [SerializeField] private float jumpForce;

    [Header("Floating spring parameters")]
    [SerializeField] private float rideHeight = 0.5f; // offset for floating spring
    [SerializeField] private float rideSpringStrength = 100f; // spring force
    [SerializeField] private float rideSpringDamper = 15f; // spring damping force

    [Header("Upright spring parameters")]
    [SerializeField] private float uprightSpringStrength = 100f; // upright spring force
    [SerializeField] private float uprightSpringDamper = 15f; // upright spring damping force

    [Header("Linear spring parameters")]
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private AnimationCurve accelerationFromDot;
    [SerializeField] private float maxAccelerationForce = 100f;
    [SerializeField] private AnimationCurve maxAccelerationForceFromDot;

    public override void Act()
    {
        Vector2 inputDirection = Game.Input.MoveInput;
        Vector2 mouseInput = Game.Input.inputActions.Player.Look.ReadValue<Vector2>();
        //Debug.Log($"inpL: {mouseInput}");
        //Debug.Log($"inpM: {inputDirection}");
        PlayerController.Raycasts.UpdateRayCastDown();
        PlayerController.MovementBehaviour.UpdateFloatingSpringPosition(PlayerController, rideHeight, rideSpringStrength, rideSpringDamper);
        PlayerController.MovementBehaviour.UpdateUprightForce(PlayerController, uprightSpringStrength, uprightSpringDamper);
        if (PlayerController.Raycasts.didRaycastHitDown)
        {
            PlayerController.VelocityChange.UpdateMovingForce(PlayerController, inputDirection, maxSpeed, acceleration, accelerationFromDot, maxAccelerationForce, maxAccelerationForceFromDot);
        }
        PlayerController.FPVrotation.Move(mouseInput);
    }
    public override void Jump()
    {
        PlayerController.MovementBehaviour.Jump(jumpForce);
    }
}
