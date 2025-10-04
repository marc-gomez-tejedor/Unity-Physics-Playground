using System;
using UnityEngine;

public enum PlayerMovementMode
{
    Grounded = 0,
    Climbing = 1,
    Swimming = 2,
    Air = 3
}

public class PlayerController : MonoBehaviour, IInitializable
{
    [Header("Input")]
    //          Input and input space
    public Transform playerInputSpace = default;
    public Vector3 playerInput;


    //          Intent
    public bool desiredJump { get; private set; }
    [SerializeField] public bool desiresClimbing;


    //          OnCollision Events
    public event Action<Collision> OnCollisionEnterEvent;
    public event Action<Collision> OnCollisionStayEvent;
    public event Action<Collider> OnTriggerEnterEvent;
    public event Action<Collider> OnTriggerStayEvent;



    [Header("States")]
    //          States Context
    //              Actions
    public PhysicsDrivenContext physicsDrivenCtx;
    public SpringDrivenContext springDrivenCtx;
    public NewSpringDrivenContext newSpringDrivenCtx;
    public SpringDrivenMothershipContext SpringDrivenMothershipCtx;
    //              Visuals
    public BallVisualContext ballVisualCtx;


    //          Public PlayerStatus and physics context
    [HideInInspector]
    public PlayerStatus Status;
    [HideInInspector]
    public PlayerContactStatus ContactStatus;


    //          Action and Visual Statemachines
    StateMachine actionsStateMachine;
    StateMachine visualsStateMachine;


    //          Serializable Action State Configs
    [SerializeField]
    PhysicsDrivenConfigSO physicsDrivenConfigSO;
    [SerializeField]
    SpringDrivenConfigSO springDrivenConfigSO;
    [SerializeField]
    NewSpringDrivenConfigSO newSpringDrivenConfigSO;
    [SerializeField]
    NewSpringDrivenConfigSO springDrivenMothershipSO;
    //          Public Action State Configs
    public PhysicsDrivenConfigSO PhysicsDrivenConfigSO => physicsDrivenConfigSO;
    public SpringDrivenConfigSO SpringDrivenConfigSO => springDrivenConfigSO;
    public NewSpringDrivenConfigSO NewSpringDrivenConfigSO => newSpringDrivenConfigSO;
    public NewSpringDrivenConfigSO SpringDrivenMothershipSO => springDrivenMothershipSO;


    //          Serializable Visuals State Configs
    [SerializeField]
    BallVisualsConfigSO ballVisualsConfigSO;
    //          Public Visuals State Configs
    public BallVisualsConfigSO BallVisualsConfigSO => ballVisualsConfigSO;

    
    public void Initialize()
    {
        //      Action State Machine
        actionsStateMachine = new StateMachine();
        PhysicsDrivenState phsxState = new PhysicsDrivenState();
        SpringDrivenState springState = new SpringDrivenState();
        NewSpringDrivenState newSpringState = new NewSpringDrivenState();
        SpringDrivenInMothership SpringInMothershipState = new SpringDrivenInMothership();

        phsxState.Init(physicsDrivenCtx);
        phsxState.AssignConfigValues(this);
        actionsStateMachine.AddState(phsxState);

        springState.Init(springDrivenCtx);
        springState.AssignConfigValues(this);
        actionsStateMachine.AddState(springState);

        newSpringState.Init(newSpringDrivenCtx);
        newSpringState.AssignConfigValues(this);
        actionsStateMachine.AddState(newSpringState);

        SpringInMothershipState.Init(SpringDrivenMothershipCtx);
        SpringInMothershipState.AssignConfigValues(this);
        actionsStateMachine.AddState(SpringInMothershipState);

        actionsStateMachine.ChangeState<PhysicsDrivenState>();  // default
        actionsStateMachine.ChangeState<SpringDrivenState>();  // new default
        actionsStateMachine.ChangeState<NewSpringDrivenState>();  // new default
        //actionsStateMachine.ChangeState<SpringDrivenInMothership>();  // new default


        //      Visual State Machine
        visualsStateMachine = new StateMachine();
        BallVisualsState ballVisualState = new BallVisualsState();

        ballVisualState.Init(ballVisualCtx);
        ballVisualState.AssignConfigValues(this);
        visualsStateMachine.AddState(ballVisualState);

        visualsStateMachine.ChangeState<BallVisualsState>();  // default
    }
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.z = Input.GetAxis("Vertical");
        playerInput.y = Input.GetAxis("UpDown");
        desiredJump = Input.GetButtonDown("Jump");
        desiresClimbing = Input.GetButton("Climb");

        actionsStateMachine.Update();
        visualsStateMachine.Update();
    }

    void FixedUpdate() => actionsStateMachine.FixedUpdate();

    void OnCollisionEnter(Collision collision)
    {
        OnCollisionEnterEvent?.Invoke(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    void OnTriggerEnter(Collider collider)
    {
        OnTriggerEnterEvent?.Invoke(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        OnTriggerStayEvent?.Invoke(collider);
    }
}
