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
    public MovementBehaviour MovementBehaviour;
    public FPVRotations FPVrotation;
    public PlayerRaycasts Raycasts;
    public RotateTowardsDesiredOrientation Orientate;
    public ComputeVelocityChange VelocityChange;
    public OnCollisionController OnCollisionController;
    public void Initialize()
    {
        if (MovementBehaviour == null) MovementBehaviour = gameObject.GetComponent<MovementBehaviour>();
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
