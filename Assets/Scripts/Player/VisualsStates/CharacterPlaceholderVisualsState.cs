using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPlaceholderVisualsState : State<CharacterPlaceholderVisualContext, PlayerController>
{
    //          PlayerController and configSO
    PlayerController player;
    CharacterPlaceholderVisualsConfigSO config;


    //          Visual transform and mesh renderer
    Transform characterModel;
    Animator animator;


    //          External info
    Vector3 upAxis;
    Vector3 forwardAxis;
    enumState playerState = enumState.None;


    //          Last contact cache
    Vector3 lastContactNormal, lastSteepNormal, lastConnectionVelocity;


    protected override void OnInit()
    {
    }

    public override void Enter() 
    {
        GameObject inst = GameObject.Instantiate(config.prefab, player.transform);
        player.Status.visualObject = inst;

        characterModel = inst.transform;
        animator = inst.GetComponent<Animator>();
    }
    public override void Update() 
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.visualsStateMachine.ChangeState<BallVisualsState>();
        }
        UpdateActionsParams();
        Vector3 normal = lastContactNormal;
        if (!player.Status.OnGround && player.Status.OnSteep)
        {
            normal = lastSteepNormal;
        }
        Vector3 movement = (Context.body.linearVelocity - lastConnectionVelocity) * Time.deltaTime;
        movement -= normal * Vector3.Dot(movement, normal);

        Quaternion rotation = characterModel.localRotation;

        Quaternion newAlignment = Quaternion.FromToRotation(characterModel.up, upAxis);
        Vector3 newLocalForward = newAlignment * characterModel.forward;
        rotation = newAlignment * rotation;
        rotation = Quaternion.FromToRotation(newLocalForward, movement.normalized) * rotation;
        characterModel.localRotation = rotation;
    }

    void UpdateActionsParams()
    {
        upAxis = player.Status.UpAxis;
        forwardAxis = player.Status.ForwardAxis;
        enumState currentPlayerState = player.Status.CurrentAttackState;
        if (currentPlayerState == enumState.None)
        {
            currentPlayerState = player.Status.CurrentMoveState;
            
        }
        if (currentPlayerState != playerState)
        {
            playerState = currentPlayerState;
            animator.SetTrigger(playerState.ToString());
        }
        lastContactNormal = player.ContactStatus.LastContactNormal;
        lastSteepNormal = player.ContactStatus.LastSteepNormal;
        lastConnectionVelocity = player.ContactStatus.LastConnectionVelocity;        
    }
    public override void FixedUpdate() { }
    public override void LateUpdate() { }

    public override void Exit()
    {
        GameObject.Destroy(player.Status.visualObject);
    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.CharacterPlaceholderVisualsConfigSO;
    }
}