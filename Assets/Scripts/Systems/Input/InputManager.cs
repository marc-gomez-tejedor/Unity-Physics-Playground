using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInitializable
{
    public static InputManager Instance { get; private set; }

    private InputSystem_Actions inputActions;

    public Vector2 MoveInput { get; private set; }

    public event Action OnJump;
    public event Action OnInteract;

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new InputSystem_Actions();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Initialize();
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;

        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Interact.performed -= OnInteractPerformed;

        inputActions.Disable();
    }
    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }
}
