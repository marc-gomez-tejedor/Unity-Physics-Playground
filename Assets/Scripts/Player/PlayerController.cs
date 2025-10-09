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
    //              Weapons
    public DefaultWeaponsContext weaponsCtx;
    //              Visuals
    public BallVisualContext ballVisualCtx;
    public BallVisualContext multiBallVisualCtx;
    public CapsuleVisualContext capsuleVisualCtx;
    //              Camera
    public OrbitCameraContext orbitCameraCtx;


    //          Public PlayerStatus and physics context
    [HideInInspector]
    public PlayerStatus Status;
    [HideInInspector]
    public PlayerContactStatus ContactStatus;


    //          Statemachines
    public StateMachine actionsStateMachine;
    public StateMachine weaponStateMachine;
    public StateMachine visualsStateMachine;
    public StateMachine camerasStateeMachine;


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


    //          Serializable Weapons State Configs
    [SerializeField]
    DefaultWeaponsConfigSO defaultWeaponsConfigSO;
    //          Public Weapons State Configs
    public DefaultWeaponsConfigSO DefaultWeaponsConfigSO => defaultWeaponsConfigSO;


    //          Serializable Visuals State Configs
    [SerializeField]
    BallVisualsConfigSO ballVisualsConfigSO;
    [SerializeField]
    BallVisualsConfigSO multiBallVisualsConfigSO;
    [SerializeField]
    CapsuleVisualsConfigSO capsuleVisualsConfigSO;
    //          Public Visuals State Configs
    public BallVisualsConfigSO BallVisualsConfigSO => ballVisualsConfigSO;
    public BallVisualsConfigSO MultiBallVisualsConfigSO => multiBallVisualsConfigSO;
    public CapsuleVisualsConfigSO CapsuleVisualsConfigSO => capsuleVisualsConfigSO;


    //          Serializable Cameras State Configs
    [SerializeField]
    OrbitCameraConfigSO orbitCameraConfigSO;
    //          Public Weapons State Configs
    public OrbitCameraConfigSO OrbitCameraConfigSO => orbitCameraConfigSO;


    public void Initialize()
    {
        InitActionsStateMachine();
        InitWeaponsStateMachine();
        InitVisualsStateMachine();
        InitCamerasStateMachine();
    }
    void InitActionsStateMachine()
    {
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

        actionsStateMachine.ChangeState<PhysicsDrivenState>();
        actionsStateMachine.ChangeState<SpringDrivenState>();
        actionsStateMachine.ChangeState<NewSpringDrivenState>();
        //actionsStateMachine.ChangeState<SpringDrivenInMothership>();
    }
    void InitWeaponsStateMachine()
    {
        weaponStateMachine = new StateMachine();

        DefaultWeaponsState dfWeaponsState = new DefaultWeaponsState();

        dfWeaponsState.Init(weaponsCtx);
        dfWeaponsState.AssignConfigValues(this);
        weaponStateMachine.AddState(dfWeaponsState);

        weaponStateMachine.ChangeState<DefaultWeaponsState>();
    }
    void InitVisualsStateMachine()
    {
        visualsStateMachine = new StateMachine();

        BallVisualsState ballVisualState = new BallVisualsState();
        BallVisualsState1 multiBallVisualState = new BallVisualsState1();
        CapsuleVisualsState capsuleBallVisualState = new CapsuleVisualsState();

        ballVisualState.Init(ballVisualCtx);
        ballVisualState.AssignConfigValues(this);
        visualsStateMachine.AddState(ballVisualState);

        multiBallVisualState.Init(ballVisualCtx);
        multiBallVisualState.AssignConfigValues(this);
        visualsStateMachine.AddState(multiBallVisualState);

        capsuleBallVisualState.Init(capsuleVisualCtx);
        capsuleBallVisualState.AssignConfigValues(this);
        visualsStateMachine.AddState(capsuleBallVisualState);

        visualsStateMachine.ChangeState<BallVisualsState>();
        visualsStateMachine.ChangeState<BallVisualsState1>();
        visualsStateMachine.ChangeState<CapsuleVisualsState>();
    }
    void InitCamerasStateMachine()
    {
        camerasStateeMachine = new StateMachine();

        OrbitCameraState orbitCameraState = new OrbitCameraState();

        orbitCameraState.Init(orbitCameraCtx);
        orbitCameraState.AssignConfigValues(this);
        camerasStateeMachine.AddState(orbitCameraState);

        camerasStateeMachine.ChangeState<OrbitCameraState>();
    }
    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.z = Input.GetAxis("Vertical");
        playerInput.y = Input.GetAxis("UpDown");
        desiredJump = Input.GetButtonDown("Jump");
        desiresClimbing = Input.GetButton("Climb");

        actionsStateMachine.Update();
        weaponStateMachine.Update();
        visualsStateMachine.Update();
        //camerasStateeMachine.Update(); // currently unused
    }

    void FixedUpdate()
    {
        actionsStateMachine.FixedUpdate();
        weaponStateMachine.FixedUpdate();
        //visualsStateMachine.FixedUpdate();  // currently unused
        //camerasStateeMachine.FixedUpdate();  // currently unused
    }
    void LateUpdate()
    {
        //actionsStateMachine.LateUpdate();  // currently unused
        //weaponStateMachine.LateUpdate();  // currently unused
        //visualsStateMachine.LateUpdate();  // currently unused
        camerasStateeMachine.LateUpdate();
    }

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
