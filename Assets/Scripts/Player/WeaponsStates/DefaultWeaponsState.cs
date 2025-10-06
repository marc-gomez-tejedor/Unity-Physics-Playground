using UnityEngine;

public class DefaultWeaponsState : State<DefaultWeaponsContext, PlayerController>
{
    //          PlayerController and configSO
    public PlayerController player;
    DefaultWeaponsConfigSO config;


    //          Raycast params
    Transform cameraTransform;
    float distance;
    LayerMask layerMask;


    //          Target
    Transform originPosition;
    Vector3 targetPosition;

    protected override void OnInit()
    {
    }
    public override void Enter()
    {
        Debug.Log($"Enter {this.GetType()}");
    }
    public override void Exit()
    {
        Debug.Log($"Exit {this.GetType()}");
    }
    public override void Update()
    {
        UpdatePlayerStatusAndContextValues();
    }
    public override void FixedUpdate()
    {
        Vector3 p = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;
        if (Physics.Raycast(p, direction, out RaycastHit hit, distance, layerMask))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = p + direction.normalized * distance;
        }
        Debug.DrawLine(originPosition.position, targetPosition, Color.magenta);
    }
    public override void LateUpdate() { }
    public void UpdatePlayerStatusAndContextValues()
    {

    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.DefaultWeaponsConfigSO;

        distance = config.maximumDistance;
        layerMask = config.mask;

        cameraTransform = Context.orbitCameraTransform;
        originPosition = Context.raycastCenterOrigin;
    }
}
