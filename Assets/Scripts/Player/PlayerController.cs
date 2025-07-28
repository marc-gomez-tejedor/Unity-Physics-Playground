using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializable
{
    [Header("References")]
    [SerializeField] private MovementBehaviour movementBehaviour;

    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpingForce;
    [SerializeField] private bool frozen = false;

    public void Initialize()
    {
        if (movementBehaviour == null)
        {
            movementBehaviour = gameObject.GetComponent<MovementBehaviour>();
        }
        movementBehaviour.Move(Vector2.zero, speed);
        InputManager.Instance.OnJump += Jump;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnJump -= Jump;
    }

    private void FixedUpdate()
    {
        if (!frozen)
        {
            Vector2 inputDirection = InputManager.Instance.MoveInput;
            movementBehaviour.Move(inputDirection, speed);
        }
    }

    private void Jump()
    {
        movementBehaviour.Jump(jumpingForce);
    }
}
