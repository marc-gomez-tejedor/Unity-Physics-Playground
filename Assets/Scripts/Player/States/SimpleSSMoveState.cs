using UnityEngine;

public class SimpleSSMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    public override void Act()
    {
        PlayerController.findEquilibrium.running = false;
        Vector2 inputDirection = Game.Input.MoveInput;
        if (PlayerController.findEquilibrium.onCollision)
        {
            Vector3 normal = PlayerController.findEquilibrium.targetObject.forward;
            PlayerController.setParent.SetParent(PlayerController.findEquilibrium.targetObject);
            PlayerController.movementBehaviour.AddSpeed(inputDirection, speed);
             
        }
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}

