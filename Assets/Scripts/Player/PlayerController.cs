using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializable
{
    [Header("Parameters")]
    public Vector3 currentFieldForce = Vector3.down; //later update to public Field currentField

    [Header("References")]
    public Rigidbody _rigidbody;
    public PlayerState CurrentState;
    public PlayerStateContainer States;
    public ForceField CurrentField;
    public FieldsContainer Fields;
    public MovementBehaviour MovementBehaviour;
    public FPVRotations FPVrotation;
    public PlayerRaycasts Raycasts;
    public RotateTowardsDesiredOrientation Orientate;
    public ComputeVelocityChange VelocityChange;
    public OnCollisionController OnCollisionController;
    public void Initialize()
    {
        if (MovementBehaviour == null) MovementBehaviour = gameObject.GetComponent<MovementBehaviour>();
        CurrentState = States.DefaultState;
        CurrentState.TransitionIn();
    }
    private void FixedUpdate()
    {
        CurrentState.Act();
    }

    public void TransitionTo(PlayerState state)
    {
        state.TransitionOut();
        CurrentState = state;
        state.TransitionIn();
    }
    
    public RaycastHit GetRaycasts()
    {
        return new RaycastHit();
    }
}
