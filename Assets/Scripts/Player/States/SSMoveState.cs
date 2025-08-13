using UnityEngine;

public class SSMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public override void Act()
    {
        base.PlayerController.findEquilibrium.Center();
        Vector2 inputDirection = Game.Input.MoveInput;
        PlayerController.movementBehaviour.AddSpeed(inputDirection, speed);
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}

