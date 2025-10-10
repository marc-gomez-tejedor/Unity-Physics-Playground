using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TargetLockedCameraState : State<TargetLockedCameraContext, PlayerController>
{
    /// <summary>
    /// make it that the center target is fixed at the hookroot
    /// then "inverse kinematic" camera pos being:
    /// align a vector such as: root -> (cameraTarget + upAxis*verticalOffset)
    /// then: camera.pos = cameraTarget + vector.normalized * cameraDistance
    /// </summary>
    PlayerController player;
    OrbitCameraConfigSO config;
 
    Vector3 focusPoint;
    Camera regularCamera;
    Transform cameraTransform;
    Transform playerTargetTransform;

    float verticalOffset;

    LayerMask obstructionMask = -1;

    float distance;


    protected override void OnInit()
    {
    }
    public override void Enter()
    {
        Debug.Log($"Enter {this.GetType()}");
        cameraTransform.localRotation = Quaternion.Euler(new Vector2(45f, 0f));
        //Subscribe();
    }
    public override void Exit()
    {
        Debug.Log($"Exit {this.GetType()}");
        //UnSubscribe();
    }
    public override void Update()
    {
        if (!player.Status.Hooking)
        {
            player.camerasStateeMachine.ChangeState<OrbitCameraState>();
        }
    }

    public override void FixedUpdate() { }

    public override void LateUpdate()
    {
        Vector3 upAxis = player.Status.UpAxis;

        Vector3 targetPoint = playerTargetTransform.position + upAxis * verticalOffset;
        Vector3 lookDirection = (focusPoint - targetPoint).normalized;
        Vector3 lookPosition = targetPoint - lookDirection * distance;

        Quaternion rotation = cameraTransform.localRotation;

        Quaternion newAlignment = Quaternion.FromToRotation(cameraTransform.up, upAxis);
        Vector3 newLocalForward = newAlignment * cameraTransform.forward;
        rotation = newAlignment * rotation;
        rotation = Quaternion.FromToRotation(newLocalForward, lookDirection) * rotation;
        
        Vector3 rectoOffset = lookDirection * regularCamera.nearClipPlane;  // check if it should be -lookdir *
        Vector3 rectPosition = lookPosition + rectoOffset;
        Vector3 castFrom = playerTargetTransform.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast
        (
            castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
            rotation, castDistance, obstructionMask, QueryTriggerInteraction.Ignore
        ))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectoOffset;
        }
        cameraTransform.SetPositionAndRotation(lookPosition, rotation);
    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.OrbitCameraConfigSO;

        focusPoint = player.Status.HookPoint;

        verticalOffset = config.verticalOffset;
        obstructionMask = config.obstructionMask;

        distance = config.distance;

        regularCamera = Context.camera;
        cameraTransform = Context.cameraTransform;
        playerTargetTransform = Context.focus;
    }

    Vector3 CameraHalfExtends
    {
        get {
            Vector3 halfExtends;
            halfExtends.y = regularCamera.nearClipPlane * 
                Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
            halfExtends.x = halfExtends.y * regularCamera.aspect;
            halfExtends.z = 0f;
            return halfExtends;
        }
    }
}
