using Unity.VisualScripting;
using UnityEngine;

public class DefaultWeaponsState : State<DefaultWeaponsContext, PlayerController>
{
    //          PlayerController and configSO
    public PlayerController player;
    DefaultWeaponsConfigSO config;


    //          Gravity spheres
    GravitySphere pushSphere;
    GravitySphere pullSphere;
    GravityRayByPlayer gravityRay;


    //          Raycast params
    Transform cameraTransform;
    float distance;
    LayerMask layerMask;


    //          Target
    Transform originPosition;
    Vector3 targetPosition;


    //          intent
    bool desiresToRay;
    enumState CurrentWeaponState = enumState.None;

    protected override void OnInit()
    {
    }
    public override void Enter()
    {
        GameObject inst = GameObject.Instantiate(config.prefab, player.transform);
        player.Status.weaponObject = inst;
        GravityQuerySettings GravityQuery = new(excludeMask: config.ExcludeMask, includeMask: config.IncludeMask);
        player.Status.playerGravityQuery = GravityQuery;
        Transform t = inst.transform.Find("pushSphere");
        pushSphere = t.GetComponent<GravitySphere>();

        t = inst.transform.Find("pullSphere");
        pullSphere = t.GetComponent<GravitySphere>();

        t = inst.transform.Find("gravRay");
        gravityRay = t.GetComponent<GravityRayByPlayer>();
        gravityRay.enabled = false;
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
        if (Input.GetKey(KeyCode.E))
        {
            desiresToRay = true;
            //Debug.Log("moouse pressed");

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
        bool enabled = false;
        if (desiresToRay)
        {
            if (player.Status.Hooking)
            {
                enabled = true;
            }
            else if (Physics.Raycast(p, direction, out RaycastHit hit, distance, layerMask))
            {
                enabled = true;
                targetPosition = hit.point;
                player.Status.HookPoint = targetPosition; 
                //player.Status.StepsSinceLastJump = -1;
                gravityRay.hitPosition = targetPosition;
            }
            else
            {
                targetPosition = p + direction.normalized * distance;
            }
        }
        gravityRay.enabled = enabled;
        player.Status.Hooking = enabled;
        //Debug.Log(enabled);
        Debug.DrawLine(originPosition.position, targetPosition, Color.magenta);
        ClearStateParams();
    }
    void ClearStateParams()
    {
        desiresToRay = false;
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
        layerMask = config.layerMask;

        cameraTransform = Context.orbitCameraTransform;
        originPosition = Context.raycastCenterOrigin;
    }
}
