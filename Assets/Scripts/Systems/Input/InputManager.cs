using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IInitializable
{
    public static InputManager Instance { get; private set; }

    public InputSystem_Actions inputActions { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public event Action OnJump;
    public event Action OnInteract;
    public event Action OnMenu;

    public void Initialize()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        inputActions = new InputSystem_Actions();
        Subscribe();
    }

    private void OnEnable()
    {
        Initialize();
        Subscribe();
    }
    private void Subscribe()
    {
        inputActions.Enable();
        UnSubscribe();

        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;

        inputActions.Player.Look.performed += OnLookPerformed;
        inputActions.Player.Look.performed += OnLookCanceled;

        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Menu.performed += OnMenuPerformed;
    }
    private void UnSubscribe()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;

        inputActions.Player.Look.performed -= OnLookPerformed;
        inputActions.Player.Look.performed -= OnLookCanceled;

        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Menu.performed -= OnMenuPerformed;
    }

    private void OnDisable()
    {
        UnSubscribe();
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
    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }
    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        LookInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }
    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }
    private void OnMenuPerformed(InputAction.CallbackContext ctx)
    {
        OnMenu?.Invoke();
    }
}
