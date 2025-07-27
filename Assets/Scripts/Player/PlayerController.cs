using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementBehaviour movementBehaviour;

    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpingForce;
    [SerializeField] private bool frozen = false;

    private void Awake()
    {
        if (movementBehaviour == null)
        {
            movementBehaviour = gameObject.GetComponent<MovementBehaviour>();
        }
    }
    private void Start()
    {
        movementBehaviour.Move(Vector2.zero, speed);
    }

    private void OnEnable()
    {
        InputManager.current.OnJump += Jump;
    }

    private void OnDisable()
    {
        InputManager.current.OnJump -= Jump;
    }

    private void FixedUpdate()
    {
        if (!frozen)
        {
            Vector2 inputDirection = InputManager.current.MoveInput;
            movementBehaviour.Move(inputDirection, speed);
        }
    }

    private void Jump()
    {
        movementBehaviour.Jump(jumpingForce);
    }
}
