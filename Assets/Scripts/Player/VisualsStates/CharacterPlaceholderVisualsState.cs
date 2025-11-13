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
        UpdateActionsParams();

        Quaternion rotation = characterModel.localRotation;

        Quaternion newAlignment = Quaternion.FromToRotation(characterModel.up, upAxis);
        Vector3 newLocalForward = newAlignment * characterModel.forward;
        rotation = newAlignment * rotation;
        rotation = Quaternion.FromToRotation(newLocalForward, forwardAxis) * rotation;
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