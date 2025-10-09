using UnityEngine;
using UnityEngine.UI;

public class BallVisualsState1 : State<BallVisualContext, PlayerController>
{
    //          PlayerController and configSO
    PlayerController player;
    BallVisualsConfigSO config;


    //          Visual ball transform and mesh renderer
    Transform ball;
    float ballRadius = 0.5f;
    MeshRenderer meshRenderer;


    //          State Materials
    Material defaultMaterial,
             climbingMaterial,
             swimmingMaterial;


    //          Rotation Parameters
    float ballAlignSpeed = 180f;
    bool ballCanReverse = false;
    float 
        ballGroundRotation = 1f,
        ballClimbingRotation = 1f,
        ballAirRotation = 0.5f,
        ballSwimRotation = 2f;


    //          Last contact cache
    Vector3 lastContactNormal, lastSteepNormal, lastConnectionVelocity;


    protected override void OnInit()
    {
    }

    public override void Enter() 
    {
        GameObject inst = GameObject.Instantiate(config.prefab, player.transform);
        player.Status.visualObject = inst;

        ball = inst.transform;
        meshRenderer = inst.GetComponent<MeshRenderer>();
    }
    public override void Update() 
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            player.visualsStateMachine.ChangeState<CapsuleVisualsState>();
        }

        UpdateActionsParams();
        Vector3 rotationPlaneNormal = lastContactNormal;
        Material ballMaterial = defaultMaterial;
        float rotatingFactor = ballGroundRotation;
        if (player.Status.Climbing)
        {
            ballMaterial = climbingMaterial;
            rotatingFactor = ballClimbingRotation;
        }
        else if (player.Status.Swimming)
        {
            ballMaterial = swimmingMaterial;
            rotatingFactor = ballSwimRotation;
        }
        else if (!player.Status.OnGround)
        {
            if (player.Status.OnSteep)
            {
                rotationPlaneNormal = lastSteepNormal;
            }
            else
            {
                rotatingFactor = ballAirRotation;
            }
        }
        meshRenderer.material = ballMaterial;

        Vector3 movement = (Context.body.linearVelocity - lastConnectionVelocity) * Time.deltaTime;
        movement -= rotationPlaneNormal * Vector3.Dot(movement, rotationPlaneNormal);
        float distance = movement.magnitude;

        Quaternion rotation = ball.localRotation;
        if (player.ContactStatus.ConnectedBody &&
            player.ContactStatus.ConnectedBody == player.ContactStatus.PreviousConnectedBody)
        {
            rotation =
                Quaternion.Euler(player.ContactStatus.ConnectedBody.angularVelocity *
                    (Mathf.Rad2Deg * Time.deltaTime))
                * rotation;
            if (distance < 0.001f)
            {
                ball.localRotation = rotation;
                return;
            }
        }
        else if (distance < 0.001f)
        {
            return;
        }
        float angle = distance * rotatingFactor * (180f / Mathf.PI) / ballRadius;
        Vector3 rotationAxis = Vector3.Cross(rotationPlaneNormal, movement).normalized;
        rotation = Quaternion.Euler(rotationAxis * angle) * rotation;
        if (ballAlignSpeed > 0f)
        {
            rotation = AlignBallRotation(rotationAxis, rotation, distance);
        }
        ball.localRotation = rotation;
    }

    void UpdateActionsParams()
    {
        lastContactNormal = player.ContactStatus.LastContactNormal;
        lastSteepNormal = player.ContactStatus.LastSteepNormal;
        lastConnectionVelocity = player.ContactStatus.LastConnectionVelocity;
    }

    Quaternion AlignBallRotation(Vector3 rotationAxis, Quaternion rotation, float traveledDistance)
    {
        Vector3 ballAxis = ball.up;
        float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        if (ballCanReverse && angle > 90f)
        {
            angle -= 90f;
            rotationAxis = -rotationAxis;
        }
        float maxAngle = ballAlignSpeed * traveledDistance;
        Quaternion newAlignment = Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
        if (angle <= maxAngle)
        {
            return newAlignment;
        }
        else
        {
            return Quaternion.SlerpUnclamped(rotation, newAlignment, maxAngle / angle);
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
        config = player.MultiBallVisualsConfigSO;

        defaultMaterial = config.defaultMaterial;
        climbingMaterial = config.climbingMaterial;
        swimmingMaterial = config.swimmingMaterial;

        ballRadius = config.ballRadius;

        ballAlignSpeed = config.ballAlignSpeed;
        ballCanReverse = config.ballCanReverse;

        ballGroundRotation = config.ballGroundRotation;
        ballClimbingRotation = config.ballClimbingRotation;
        ballAirRotation = config.ballAirRotation;
        ballSwimRotation = config.ballSwimRotation;
    }
}