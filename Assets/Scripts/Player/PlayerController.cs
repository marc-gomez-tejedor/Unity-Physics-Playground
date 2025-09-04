using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializable
{
    [Header("Parameters")]
    public Vector3 currentFieldForce = Vector3.down; //later update to public Field currentField

    [Header("References")]
    public Rigidbody _rigidbody;
    public PlayerState State;
    public PlayerStateContainer States;
    public MovementBehaviour movementBehaviour;
    public FPVRotations FPVrotation;
    public RotateTowardsDesiredOrientation Orientate;
    public PlayerRaycasts raycasts;
    //public SimpleForceApplier simpleForceApplier;
    
    public void Initialize()
    {
        if (movementBehaviour == null) movementBehaviour = gameObject.GetComponent<MovementBehaviour>();
        State = States.DefaultState;
        State.TransitionIn();
    }
    private void FixedUpdate()
    {
        State.Act();
    }

    public void TransitionTo(PlayerState state)
    {
        state.TransitionOut();
        State = state;
        state.TransitionIn();
    }
    
    public RaycastHit GetRaycasts()
    {
        return new RaycastHit();
    }
}
