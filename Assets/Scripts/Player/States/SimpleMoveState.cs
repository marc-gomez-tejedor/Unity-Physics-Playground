using UnityEngine;

public class SimpleMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public override void TransitionIn()
    {
        base.OnEnable();
    }
    public override void Act()
    {
        Vector2 inputDirection = Game.Input.MoveInput;
        PlayerController.movementBehaviour.Move(inputDirection, speed);
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}
