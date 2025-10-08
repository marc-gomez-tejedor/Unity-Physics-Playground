using UnityEngine;

public class DefaultWeaponsState : State<DefaultWeaponsContext, PlayerController>
{
    //          PlayerController and configSO
    public PlayerController player;
    DefaultWeaponsConfigSO config;


    //          Gravity spheres
    GravitySphere pushSphere;
    GravitySphere pullSphere;


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
        GameObject inst = GameObject.Instantiate(config.prefab, player.transform);
        player.Status.weaponObject = inst;
        Transform t = inst.transform.Find("pushSphere");
        pushSphere = t.GetComponent<GravitySphere>();

        t = inst.transform.Find("pullSphere");
        pullSphere = t.GetComponent<GravitySphere>();
        //Debug.Log($"Enter {this.GetType()}");
    }
    public override void Exit()
    {
        GameObject.Destroy(player.Status.weaponObject);
        //Debug.Log($"Exit {this.GetType()}");
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwapPullSphere();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwapPushSphere();
        }
        UpdatePlayerStatusAndContextValues();
    }
    void SwapPullSphere()
    {
        if (pullSphere.enabled)
        {
            pullSphere.enabled = false;
            Debug.Log("disabled");
            Debug.Log(pullSphere.enabled);
            return;
        }
        pullSphere.enabled = true;
        Debug.Log("enabled");
        Debug.Log(pullSphere.enabled);
    }
    void SwapPushSphere()
    {
        if (pushSphere.enabled)
        {
            pushSphere.enabled = false;
                return;
        }
        pushSphere.enabled = true;
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
