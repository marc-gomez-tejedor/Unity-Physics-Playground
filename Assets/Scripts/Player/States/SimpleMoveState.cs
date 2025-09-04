using UnityEngine;

public class SimpleMoveState : PlayerState
{
    [Header("General Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("spring parameters")]
    [SerializeField] private float RideHeight = 0.5f; // offset for floating spring
    [SerializeField] private float RideSpringStrength = 100f; // spring force
    [SerializeField] private float RideSpringDamper = 15f; // spring damping force

    [Header("upright spring parameters")]
    [SerializeField] private float UprightSpringStrength = 100f; // upright spring force
    [SerializeField] private float UprightSpringDamper = 15f; // upright spring damping force

    public override void Act()
    {
        Vector2 inputDirection = Game.Input.MoveInput;
        Vector2 mouseInput = Game.Input.inputActions.Player.Look.ReadValue<Vector2>();
        Debug.Log($"inpL: {mouseInput}");
        Debug.Log($"inpM: {inputDirection}");
        PlayerController.FPVrotation.Move(mouseInput);
        PlayerController.movementBehaviour.UpdateFloatingSpringPosition(PlayerController, RideHeight, RideSpringStrength, RideSpringDamper);        
        PlayerController.movementBehaviour.UpdateUprightForce(PlayerController, UprightSpringStrength, UprightSpringDamper);
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}
