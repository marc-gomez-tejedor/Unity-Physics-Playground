using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleVisualsState : State<CapsuleVisualContext, PlayerController>
{
    //          PlayerController and configSO
    PlayerController player;
    CapsuleVisualsConfigSO config;


    //          Visual transform and mesh renderer
    Transform capsule;
    MeshRenderer meshRenderer;


    //          State Materials
    Material defaultMaterial,
             climbingMaterial,
             swimmingMaterial;


    //          External info
    Vector3 upAxis;
    Vector3 forwardAxis;


    protected override void OnInit()
    {
    }

    public override void Enter() 
    {
        GameObject inst = GameObject.Instantiate(config.prefab, player.transform);
        player.Status.visualObject = inst;

        capsule = inst.transform;
        meshRenderer = inst.GetComponent<MeshRenderer>();
    }
    public override void Update() 
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.visualsStateMachine.ChangeState<BallVisualsState>();
        }

        UpdateActionsParams();
        Material material = defaultMaterial;
        if (player.Status.Climbing)
        {
            material = climbingMaterial;
        }
        else if (player.Status.Swimming)
        {
            material = swimmingMaterial;
        }
        meshRenderer.material = material;

        Quaternion rotation = capsule.localRotation;

        Quaternion newAlignment = Quaternion.FromToRotation(capsule.up, upAxis);
        Vector3 newLocalForward = newAlignment * capsule.forward;
        rotation = newAlignment * rotation;
        rotation = Quaternion.FromToRotation(newLocalForward, forwardAxis) * rotation;
        capsule.localRotation = rotation;
    }

    void UpdateActionsParams()
    {
        upAxis = player.Status.UpAxis;
        forwardAxis = player.Status.ForwardAxis;
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
        config = player.CapsuleVisualsConfigSO;

        defaultMaterial = config.defaultMaterial;
        climbingMaterial = config.climbingMaterial;
        swimmingMaterial = config.swimmingMaterial;
    }
}