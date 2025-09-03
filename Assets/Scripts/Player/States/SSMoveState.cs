using UnityEngine;

public class SSMoveState : PlayerState
{
    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform mothershipTransform;

    public override void Act()
    {
        UpdateCurrentFieldForce();
        PlayerController.Orientate.Center();
        Vector2 inputDirection = Game.Input.MoveInput;
        //PlayerController.movementBehaviour.AddSpeed(inputDirection, speed);
    }
    public void UpdateCurrentFieldForce()
    {
        Vector3 center = new Vector3(PlayerController._rigidbody.transform.position.x, mothershipTransform.position.y, mothershipTransform.position.z);
        PlayerController.currentFieldForce = (PlayerController._rigidbody.transform.position - center).normalized;
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}

