using System;
using Unity.VisualScripting;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    [Header("Parameters")]
    public Vector3 currentFieldForce = Vector3.down; //later update to public Field currentField

    [Header("References")]
    public Rigidbody _rigidbody;
    public OldPlayerState CurrentState;
    public OldPlayerStateContainer States;
    public OldForceField CurrentField;
    public OldFieldsContainer Fields;
    public OldMovementBehaviour MovementBehaviour;
    public OldFPVRotations FPVrotation;
    public OldPlayerRaycasts Raycasts;
    public OldRotateTowardsDesiredOrientation Orientate;
    public OldComputeVelocityChange VelocityChange;
    public OldOnCollisionController OnCollisionController;

    public void Awake()
    {
        if (MovementBehaviour == null) MovementBehaviour = gameObject.GetComponent<OldMovementBehaviour>();
        CurrentState = States.DefaultState;
        CurrentState.TransitionIn();
    }
    private void Update()
    {
        CurrentState.UpdateInput();
    }
    private void FixedUpdate()
    {
        CurrentState.Act();
    }
    private void LateUpdate()
    {
        CurrentState.CameraControl();
    }
    public void TransitionTo(OldPlayerState state)
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
