using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCameraState : State<OrbitCameraContext, PlayerController>
{
    PlayerController player;
    OrbitCameraConfigSO config;
 
    Transform focus = default;
    Camera regularCamera;
    Transform cameraTransform;

    float verticalOffset;
    Vector3 focusPoint, previousFocusPoint;

    LayerMask obstructionMask = -1;

    float distance;
    float focusRadius;
    float focusCentering;

    Vector2 orbitAngles = new Vector2(45f, 0f);
    float rotationSpeed;
    float minVerticalAngle, maxVerticalAngle;

    float alignDelay;
    float alignSmoothRange;
    float lastManualRotationTime;

    float upAlignmentSpeed;

    Quaternion gravityAlignment = Quaternion.identity;

    Quaternion orbitRotation;


    protected override void OnInit()
    {
    }
    public override void Enter()
    {
        Debug.Log($"Enter {this.GetType()}");
        maxVerticalAngle = Mathf.Max(maxVerticalAngle, minVerticalAngle);
        focusPoint = focus.position + player.Status.UpAxis * verticalOffset;
        cameraTransform.localRotation = orbitRotation = Quaternion.Euler(orbitAngles);
        lastManualRotationTime = Time.unscaledTime;
        //Subscribe();
    }
    public override void Exit()
    {
        Debug.Log($"Exit {this.GetType()}");
        //UnSubscribe();
    }
    public override void Update() { }

    public override void FixedUpdate() { }

    public override void LateUpdate()
    {
        UpdateGravityAlignment();
        UpdateFocusPoint();
        
        if (ManualRotation() || AutomaticRotation())
        {
            ConstrainAngles();
            orbitRotation = Quaternion.Euler(orbitAngles);
        }
        Quaternion lookRotation = gravityAlignment * orbitRotation;

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;

        Vector3 rectoOffset = lookDirection * regularCamera.nearClipPlane;
        Vector3 rectPosition = lookPosition + rectoOffset;
        Vector3 castFrom = focus.position;
        Vector3 castLine = rectPosition - castFrom;
        float castDistance = castLine.magnitude;
        Vector3 castDirection = castLine / castDistance;

        if (Physics.BoxCast
        (
            castFrom, CameraHalfExtends, castDirection, out RaycastHit hit, 
            lookRotation, castDistance, obstructionMask, QueryTriggerInteraction.Ignore
        ))
        {
            rectPosition = castFrom + castDirection * hit.distance;
            lookPosition = rectPosition - rectoOffset;
        }
        cameraTransform.SetPositionAndRotation(lookPosition, lookRotation);
    }
    void UpdateFocusPoint()
    {
        previousFocusPoint = focusPoint;
        Vector3 targetPoint = focus.position + player.Status.UpAxis * verticalOffset;
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            float t = 1f;
            if (distance > 0.01f && focusCentering > 0f)
            {
                t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
            }
            if (distance > focusRadius)
            {
                t = Mathf.Min(t, focusRadius / distance);
            }
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
        }
        else
        {
            focusPoint = targetPoint;
        }            
    }

    bool ManualRotation()
    {
        Vector2 input = new Vector2
        (
            Input.GetAxis("Vertical Camera"),
            Input.GetAxis("Horizontal Camera")
        );
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    bool AutomaticRotation()
    {
        if (Time.unscaledTime - lastManualRotationTime < alignDelay)
        {
            return false;
        }
        Vector3 alignedDelta = Quaternion.Inverse(gravityAlignment) * (focusPoint - previousFocusPoint);
        Vector2 movement = new Vector2(alignedDelta.x, alignedDelta.z);
        float movementDeltaSqr = movement.sqrMagnitude;
        if (movementDeltaSqr < 0.0001f)
        {
            return false;
        }

        float headingAngle = MathUtils.GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
        float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
        float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
        if (deltaAbs < alignSmoothRange)
        {
            rotationChange *= deltaAbs / alignSmoothRange;
        }
        else if (180f - deltaAbs < alignSmoothRange)
        {
            rotationChange *= (180f - deltaAbs) / alignSmoothRange;
        }
        orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
        return true;
    }
    void UpdateGravityAlignment()
    {
        Vector3 fromUp = gravityAlignment * Vector3.up;
        Vector3 toUp = CustomGravity.GetUpAxis(focusPoint, GravityType.GravityCastedByPlayer);
        float dot = Mathf.Clamp(Vector3.Dot(fromUp, toUp), -1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float maxAngle = upAlignmentSpeed * Time.deltaTime;
        Quaternion newAlignment = Quaternion.FromToRotation(fromUp, toUp) * gravityAlignment;
        if (angle <= maxAngle)
        {
            gravityAlignment = newAlignment;
        }
        else
        {
            gravityAlignment = Quaternion.SlerpUnclamped(gravityAlignment, newAlignment, maxAngle / angle);
        }
    }
    void ConstrainAngles()
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);
        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.OrbitCameraConfigSO;

        verticalOffset = config.verticalOffset;
        obstructionMask = config.obstructionMask;

        distance = config.distance;
        focusRadius = config.focusRadius;
        focusCentering = config.focusCentering;

        rotationSpeed = config.rotationSpeed;
        minVerticalAngle = config.minVerticalAngle;
        maxVerticalAngle = config.maxVerticalAngle;

        alignDelay = config.alignDelay;
        alignSmoothRange = config.alignSmoothRange;
        upAlignmentSpeed = config.upAlignmentSpeed;

        regularCamera = Context.camera;
        cameraTransform = Context.cameraTransform;
        focus = Context.focus;
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
