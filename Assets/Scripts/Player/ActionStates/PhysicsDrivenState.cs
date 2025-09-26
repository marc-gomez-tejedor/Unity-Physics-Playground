using UnityEngine;

public class PhysicsDrivenState : State<PhysicsDrivenContext, PlayerController>
{
    //          PlayerController and configSO
    public PlayerController player;
    PhysicsDrivenConfigSO config;


    //      ***later update to spring parameters***
    //          Movement tuning
    float maxAcceleration,
          maxAirAcceleration,
          maxClimbAcceleration,
          maxSwimAcceleration;


    //          Intent
    bool desiredJump;
    bool desiresClimbing;


    //          Speed caps
    float maxSpeed,
          maxClimbSpeed,
          maxSwimSpeed;


    //          Velocity and connected body velocity info (runtime)
    Vector3 velocity,
            connectionVelocity;


    //          Jump params
    float jumpHeight;
    int maxAirJumps;


    //          Jump & Contact counters (runtime)
    int jumpPhase;
    int groundContactCount,
        steepContactCount,
        climbContactCount;


    //          Climb tuning
    float maxClimbAngle;


    //          Derived boolean properties
    bool OnGround => groundContactCount > 0;
    bool OnSteep => steepContactCount > 0;
    bool Climbing => climbContactCount > 0 && stepsSinceLastJump > 2;
    bool InWater => submergence > 0f; 
    bool Swimming => submergence >= swimThreshold;  // later move to swimming state


    //          Timing counters
    int stepsSinceLastGrounded,
        stepsSinceLastJump;
    float submergence;


    //          Probing & snap params
    float maxSnapSpeed;
    float probeDistance;
    LayerMask probeMask = -1,
              stairsMask = -1,
              climbMask = -1,
              waterMask = 0;


    //          Angle limits & precomputed values
    float maxGroundAngle,
          maxStairsAngle;
    float minGroundDotProduct,
          minStairsDotProduct,
          minClimbDotProduct;


    //          Contact normals
    Vector3 contactNormal,
            steepNormal,
            climbNormal,
            lastClimbNormal;


    //          Input and Axes
    Vector3 input;
    Vector3 upAxis;
    Vector3 rightAxis;
    Vector3 forwardAxis;


    //          Connection position tracking
    Vector3 connectionWorldPosition,
            connectionLocalPosition;


    //          Water-specific tuning
    float submergenceOffset;
    float submergenceRange;
    float waterDrag;
    float buoyancy;
    float swimThreshold;


    //          Previous-frame caches
    Vector3 lastContactNormal,
            lastSteepNormal,
            lastConnectionVelocity;


    Rigidbody body,
              connectedBody,
              previousConnectedBody;

    
    protected override void OnInit()
    {
        body = Context.body;
        body.useGravity = false;
    }
    public override void Enter()
    {
        Debug.Log($"Enter {this.GetType()}");
        Subscribe();
    }
    public override void Exit()
    {
        Debug.Log($"Exit {this.GetType()}");
        UnSubscribe();
    }
    public override void Update()
    {
        input.x = player.playerInput.x;
        input.z = player.playerInput.z;
        input.y = Swimming ? player.playerInput.y : 0f;
        input = Vector3.ClampMagnitude(input, 1f);

        if (player.playerInputSpace)
        {
            rightAxis = MathUtils.ProjectDirectionOnContactPlane(player.playerInputSpace.right, upAxis);
            forwardAxis = MathUtils.ProjectDirectionOnContactPlane(player.playerInputSpace.forward, upAxis);
        }
        else
        {
            rightAxis = MathUtils.ProjectDirectionOnContactPlane(Vector3.right, upAxis);
            forwardAxis = MathUtils.ProjectDirectionOnContactPlane(Vector3.forward, upAxis);
        }
        if (Swimming)
        {
            desiresClimbing = false;
        }
        else
        {
            desiredJump |= player.desiredJump;
            desiresClimbing = player.desiresClimbing;
        }

        UpdatePlayerStatusAndContextValues();
    }
    public override void FixedUpdate()
    {
        UpdateStateParams();

        UpdateVelocity();
        body.linearVelocity = velocity;

        ClearStateParams();
    }
    void UpdateStateParams()
    {
        stepsSinceLastGrounded++;
        stepsSinceLastJump++;
        velocity = body.linearVelocity;
        if (CheckClimbing() || CheckSwimming() ||
            OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1)
            {
                jumpPhase = 0;
            }
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = upAxis;
        }
        if (connectedBody)
        {
            if (connectedBody.isKinematic || connectedBody.mass >= body.mass)
            {
                UpdateConnectionState();
            }
        }
    }
    void ClearStateParams()
    {
        lastContactNormal = contactNormal;
        lastSteepNormal = steepNormal;
        lastConnectionVelocity = connectionVelocity;
        groundContactCount = steepContactCount = climbContactCount = 0;
        contactNormal = steepNormal = climbNormal = Vector3.zero;
        connectionVelocity = Vector3.zero;
        previousConnectedBody = connectedBody;
        connectedBody = null;
        submergence = 0f;
    }
    void UpdateVelocity()
    {
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);

        if (InWater)
        {
            velocity *= 1f - waterDrag * submergence * Time.deltaTime;
        }
        AdjustVelocity();

        if (desiredJump)
        {
            desiredJump = false;
            Jump(gravity);
        }

        if (Climbing)
        {
            velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
        }
        else if (InWater)
        {
            velocity += gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
        }
        else if (OnGround && velocity.sqrMagnitude < 0.01f)
        {
            velocity += contactNormal * (Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
        }
        else if (desiresClimbing && OnGround)
        {
            velocity += (gravity - contactNormal * (maxClimbAcceleration * 0.9f)) * Time.deltaTime;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
    }
    bool CheckClimbing()
    {
        if (Climbing)
        {
            if (climbContactCount > 1)
            {
                climbNormal.Normalize();
                float upDot = Vector3.Dot(upAxis, climbNormal);
                if (upDot >= minGroundDotProduct)
                {
                    climbNormal = lastClimbNormal;
                }
            }
            groundContactCount = 1;
            contactNormal = climbNormal;
            return true;
        }
        return false;
    }
    void AdjustVelocity()
    {
        float acceleration, speed;
        Vector3 xAxis, zAxis;
        if (Climbing)
        {
            acceleration = maxClimbAcceleration;
            speed = maxClimbSpeed;
            xAxis = Vector3.Cross(contactNormal, upAxis);
            zAxis = upAxis;
        }
        else if (InWater)
        {
            float swimFactor = Mathf.Min(1f, submergence / swimThreshold);
            acceleration = Mathf.LerpUnclamped(OnGround ? maxAcceleration : maxAirAcceleration,
                maxSwimAcceleration, swimFactor);
            speed = Mathf.LerpUnclamped(maxSpeed, maxSwimSpeed, swimFactor);
            xAxis = rightAxis;
            zAxis = forwardAxis;
        }
        else
        {
            acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
            speed = OnGround && desiresClimbing ? maxClimbSpeed : maxSpeed;
            xAxis = rightAxis;
            zAxis = forwardAxis;
        }
        xAxis = MathUtils.ProjectDirectionOnContactPlane(xAxis, contactNormal);
        zAxis = MathUtils.ProjectDirectionOnContactPlane(zAxis, contactNormal);

        Vector3 relativeVelocity = velocity - connectionVelocity;

        Vector3 adjustment;
        adjustment.x = input.x * speed - Vector3.Dot(relativeVelocity, xAxis);
        adjustment.z = input.z * speed - Vector3.Dot(relativeVelocity, zAxis);
        adjustment.y = Swimming ? input.y * speed - Vector3.Dot(relativeVelocity, upAxis) : 0f;

        adjustment = Vector3.ClampMagnitude(adjustment, acceleration * Time.deltaTime);

        velocity += xAxis * adjustment.x + zAxis * adjustment.z;
        if (Swimming)
        {
            velocity += upAxis * adjustment.y;
        }
    }
    void UpdateConnectionState()
    {
        if (connectedBody == previousConnectedBody)
        {
            Vector3 connectionMovement =
                connectedBody.transform.TransformPoint(connectionLocalPosition) -
                connectionWorldPosition;
            connectionVelocity = connectionMovement / Time.deltaTime;
        }
        connectionWorldPosition = body.position;
        connectionLocalPosition = connectedBody.transform.InverseTransformPoint(connectionWorldPosition);
    }
    void Jump(Vector3 gravity)
    {
        Vector3 jumpDirection;
        if (OnGround)
        {
            jumpDirection = contactNormal;
        }
        else if (OnSteep)
        {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        }
        else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps)
        {
            if (jumpPhase == 0)
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        else
        {
            return;
        }

        stepsSinceLastJump = 0;
        jumpPhase++;

        float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
        if (InWater)
        {
            jumpSpeed *= Mathf.Max(0f, 1f - submergence / swimThreshold);
        }
        jumpDirection = (jumpDirection + upAxis).normalized;
        float alignedSpeed = Vector3.Dot(velocity, jumpDirection);

        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        velocity += jumpDirection * jumpSpeed;
    }
    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2 || InWater)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast
            (
                body.position, -upAxis, out RaycastHit hit,
                probeDistance, probeMask, QueryTriggerInteraction.Ignore
            ))
        {
            return false;
        }
        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }
        groundContactCount = 1;
        contactNormal = hit.normal;

        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }
        connectedBody = hit.rigidbody;
        return true;
    }
    public void PreventSnapToGround()
    {
        stepsSinceLastJump = -1;
    }
    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }
    bool CheckSwimming()
    {
        if (Swimming)
        {
            groundContactCount = 0;
            contactNormal = upAxis;
            return true;
        }
        return false;
    }
    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }

    void OnEnable()
    {
        if(player) Subscribe();
    }
    void OnDisable()
    {
        UnSubscribe();
    }

    void Subscribe()
    {
        UnSubscribe();
        player.OnCollisionEnterEvent += CollisionEnter;
        player.OnCollisionStayEvent += CollisionStay;
        player.OnTriggerEnterEvent += TriggerEnter;
        player.OnTriggerStayEvent += TriggerStay;
    }

    void UnSubscribe()
    {
        player.OnCollisionEnterEvent -= CollisionEnter;
        player.OnCollisionStayEvent -= CollisionStay;
        player.OnTriggerEnterEvent -= TriggerEnter;
        player.OnTriggerStayEvent -= TriggerStay;
    }
    void CollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void CollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }
    void TriggerEnter(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence(other);
        }
    }
    void TriggerStay(Collider other)
    {
        if ((waterMask & (1 << other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence(other);
        }
    }

    void EvaluateCollision(Collision collision)
    {
        if (Swimming)
        {
            return;
        }
        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot)
            {
                groundContactCount++;
                contactNormal += normal;
                connectedBody = collision.rigidbody;
            }
            else
            {
                if (upDot > -0.01f)
                {
                    steepContactCount++;
                    steepNormal += normal;
                    if (groundContactCount == 0)
                    {
                        connectedBody = collision.rigidbody;
                    }
                }
                if (desiresClimbing && upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0)
                {
                    climbContactCount++;

                    Debug.Log(climbContactCount);
                    climbNormal += normal;
                    lastClimbNormal = normal;
                    connectedBody = collision.rigidbody;
                }
            }
        }
    }
    void EvaluateSubmergence(Collider collider)
    {
        if (Physics.Raycast
        (
            body.position + upAxis * submergenceOffset,
            -upAxis, out RaycastHit hit, submergenceRange + 1f,
            waterMask, QueryTriggerInteraction.Collide
        ))
        {
            submergence = 1f - hit.distance / submergenceRange;
        }
        else
        {
            submergence = 1f;
        }
        if (Swimming)
        {
            connectedBody = collider.attachedRigidbody;
        }
    }
    public override void LateUpdate() { }
    public void UpdatePlayerStatusAndContextValues()
    {
        player.Status.OnGround    = OnGround;
        player.Status.OnSteep     = OnSteep;
        player.Status.Climbing    = Climbing;
        player.Status.InWater     = InWater;
        player.Status.Swimming    = Swimming;
        player.Status.Submergence = submergence;
        player.Status.StepsSinceLastGrounded = stepsSinceLastGrounded;

        player.PhysicsContext.ConnectedBody          = connectedBody;
        player.PhysicsContext.PreviousConnectedBody  = previousConnectedBody;
        player.PhysicsContext.LastConnectionVelocity = lastConnectionVelocity;
        player.PhysicsContext.LocalGroundNormal      = contactNormal;
        player.PhysicsContext.LastContactNormal      = lastContactNormal;
        player.PhysicsContext.LastSteepNormal        = lastSteepNormal;
    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.PhysicsDrivenConfigSO;

        maxAcceleration = config.maxAcceleration;
        maxAirAcceleration = config.maxAirAcceleration;
        maxClimbAcceleration = config.maxClimbAcceleration;
        maxSwimAcceleration = config.maxSwimAcceleration;

        maxSpeed = config.maxSpeed;
        maxClimbSpeed = config.maxClimbSpeed;
        maxSwimSpeed= config.maxSwimSpeed;

        jumpHeight = config.jumpHeight;
        maxAirJumps = config.maxAirJumps;

        maxClimbAngle = config.maxClimbAngle;

        maxSnapSpeed = config.maxSnapSpeed;
        probeDistance = config.probeDistance;
        probeMask = config.probeMask;
        stairsMask = config.stairsMask;
        climbMask = config.climbMask;
        waterMask = config.waterMask;

        maxGroundAngle = config.maxGroundAngle;
        maxStairsAngle = config.maxStairsAngle;

        submergenceOffset = config.submergenceOffset;
        submergenceRange = config.submergenceRange;
        waterDrag = config.waterDrag;
        buoyancy = config.buoyancy;
        swimThreshold = config.swimThreshold;

        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
        minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
    }
}
