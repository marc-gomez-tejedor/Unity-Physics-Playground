using UnityEngine;

public class SimpleMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public override void Act()
    {
        Vector2 inputDirection = Game.Input.MoveInput;
        Vector2 mouseInput = Game.Input.inputActions.Player.Look.ReadValue<Vector2>();
        Debug.Log($"inpL: {mouseInput}");
        Debug.Log($"inpM: {inputDirection}");
        PlayerController.FPVrotation.Move(mouseInput);
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}
