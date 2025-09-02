using UnityEngine;

public class SSMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public override void Act()
    {
        PlayerController.Orientate.Center();
        Vector2 inputDirection = Game.Input.MoveInput;
        PlayerController.movementBehaviour.AddSpeed(inputDirection, speed);
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}

