using Unity.VisualScripting;
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
    //          Input and input space
    public Transform playerInputSpace = default;
    public Vector3 playerInput;


    //          Public PlayerStatus and physics context
    public PlayerStatus Status;
    public PlayerPhysicsContext PhysicsContext;


    //          Intent
    public bool desiredJump {  get; private set; }
    public bool desiresClimbing {  get; private set; }


    public Rigidbody body;


    //          Action and Visual Statemachines
    public StateMachine<PlayerController> actionsStateMachine;
    public StateMachine<PlayerController> visualsStateMachine;


    //          Serializable Action State Configs
    [SerializeField]
    PhysicsDrivenConfigSO physicsDrivenConfigSO;
    //          Public Action State Configs
    public PhysicsDrivenConfigSO PhysicsDrivenConfigSO => physicsDrivenConfigSO;


    //          Serializable Visuals State Configs
    [SerializeField]
    BallVisualsConfigSO ballVisualsConfigSO;
    //          Public Visuals State Configs
    public BallVisualsConfigSO BallVisualsConfigSO => ballVisualsConfigSO;


    public void Initialize()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;


        //      Action State Machine
        actionsStateMachine = new StateMachine<PlayerController>();

        actionsStateMachine.AddState(new PhysicsDrivenState(), this);

        actionsStateMachine.ChangeState<PhysicsDrivenState>();  // default


        //      Visual State Machine
        visualsStateMachine = new StateMachine<PlayerController>();

        visualsStateMachine.AddState(new BallVisualsState(), this);
        
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
    }

    void FixedUpdate() => actionsStateMachine.FixedUpdate();
}
