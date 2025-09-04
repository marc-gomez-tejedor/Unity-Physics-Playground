using UnityEngine;

public class SimpleSSMoveState : PlayerState
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
        PlayerController.cameraMovementBehaviour.Move(mouseInput);
        if (PlayerController.Orientate.onCollision)
        {
            Transform target = PlayerController.Orientate.targetObject;
            if (PlayerController.transform.parent != target)
            {
                PlayerController.setParent.SetTo(target);
            }
            //PlayerController.movementBehaviour.Move3D(inputDirection, speed);
        }
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
    public override void TransitionIn()
    {
        //PlayerController._rigidbody.isKinematic = true;
    }
    public override void TransitionOut()
    {
        //PlayerController._rigidbody.isKinematic = false;
    }
}

