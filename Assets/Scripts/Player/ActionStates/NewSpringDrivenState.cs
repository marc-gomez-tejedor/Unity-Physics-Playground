using System.Collections.Generic;
using UnityEngine;

public class NewSpringDrivenState : State<NewSpringDrivenContext, PlayerController>
{
    //          PlayerController and configSO
    public PlayerController player;
    NewSpringDrivenConfigSO config;


    //      ***later update to spring parameters***
    //          Movement tuning
    float maxAcceleration,
          maxAirAcceleration,
          maxClimbAcceleration,
          maxSwimAcceleration;


    //          Floating Spring Params
    float rideHeight,
        rideSpringStrength,
        rideSpringDamper;


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
    Transform raycastOrigin;
    float downRayDistance, fwdRayDistance;
    LayerMask probeMask = -1,
              stairsMask = -1,
              climbMask = -1,
              waterMask = 0;
    float downBoxDistance, fwdBoxDistance;
    bool didDownHit, didFwdHit;
    RaycastHit downRayHit, fwdRayHit;

    Vector3 downHalfExtents;  // flat "pancake"
    Vector3 fwdHalfExtents;  // flat "pancake"


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


    //          Rigidbody and connected cache
    Rigidbody body,
              connectedBody,
              previousConnectedBody;
    List<Rigidbody> connectedBodies = new List<Rigidbody>();


    protected override void OnInit()
    {
        body = Context.body;
        body.useGravity = false;
        raycastOrigin = Context.raycastOrigin;
    }
    public override void Enter()
    {
        Debug.Log($"Enter {this.GetType()}");
        //Subscribe();
    }
    public override void Exit()
    {
        Debug.Log($"Exit {this.GetType()}");
        //UnSubscribe();
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
        // vel = rigidbody.vel
        // getGrav -> desiredVel to axis -> raycasts ->
        // Swimming -> Jumping -> OnClimb ->
        // OnGround -> else -> OnAir ->
        // connectedVel and relVel -> transpose to plane ->
        // addvel to rigidbody 
                
        // cast sphereCasts and set Swimming, Onground, Climb, etc
        UpdateStateParams();

        UpdateVelocity();
        body.linearVelocity = velocity;
        player.Status.StepsSinceLastJump = stepsSinceLastJump;

        ClearStateParams();
    }
    void EvaluateRaycast(RaycastHit hit)
    {
        int layer = hit.collider.gameObject.layer;
        Vector3 normal = hit.normal;
        float upDot = Vector3.Dot(upAxis, normal);
        // check if its not steeper than maxGroundAngle
        if (upDot >= minGroundDotProduct)
        {
            groundContactCount++;
            contactNormal += normal;
            connectedBody = hit.rigidbody;
        }
        else
        {
            // else if its steep or a wall
            if (upDot > -0.01f)
            {
                steepContactCount++;
                steepNormal += normal;
                if (groundContactCount == 0)
                {
                    connectedBody = hit.rigidbody;
                }
            }
            if (desiresClimbing && upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0)
            {
                climbContactCount++;

                climbNormal += normal;
                lastClimbNormal = normal;
                connectedBody = hit.rigidbody;
            }
        }
        if (connectedBody)
        {
            if (connectedBody.isKinematic || connectedBody.mass >= body.mass)
            {
                UpdateConnectionState();
            }
        }
    }
    /// <TODO>
    /// FIX BOXCAST NOT WORKING -SEARCH UP SOME BOXCAST TUTORIAL-
    /// LATER ON KEEP ADDING MORE MECHANICS:
    ///     ADD FWD CAST AGAIN -> USE IT TO GETCLIMB 
    ///     -> CLIMB:FLOATSPRINGFORCE TO WALL
    /// </TODO>
    void UpdateStateParams()
    {
        // get current velocity
        velocity = body.linearVelocity;

        // get upAxis
        upAxis = CustomGravity.GetUpAxis(body.position);
        
        // cache params
        stepsSinceLastGrounded++;
        stepsSinceLastJump = player.Status.StepsSinceLastJump;
        stepsSinceLastJump++;
        didDownHit = false; didFwdHit = false;

        // downwards raycast
        Debug.DrawLine(raycastOrigin.position, raycastOrigin.position - upAxis * downRayDistance, Color.yellow);
        didDownHit = MathRaycasts.GetBoxInfo(raycastOrigin.position, -upAxis, downRayDistance,
            downBoxDistance, downHalfExtents, probeMask, out downRayHit);
        if (didDownHit && stepsSinceLastJump > 10)
        {
            stepsSinceLastGrounded = 0;
            EvaluateRaycast(downRayHit);
        }
        else
        {
            contactNormal = upAxis;
        }
        // forwards raycast
        //Debug.DrawLine(raycastOrigin.position, raycastOrigin.position + forwardAxis * fwdRayDistance, Color.yellow);
        //didDownHit = MathRaycasts.GetBoxInfo(raycastOrigin.position, forwardAxis, fwdRayDistance,
        //    fwdBoxDistance, downHalfExtents, probeMask, out fwdRayHit);
        //if (didFwdHit)
        //{
        //    EvaluateRaycast(fwdRayHit);
        //}
        /*if (CheckClimbing() || CheckSwimming() ||
            OnGround || SnapToGround() || CheckSteepContacts())
        {*/
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


        connectedBodies.Clear();
    }
    void UpdateVelocity()
    {
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);

        Debug.Log($"vel pre update {velocity}");
        if (InWater)
        {
            velocity *= 1f - waterDrag * submergence * Time.deltaTime;
        }
        ApplyVelocityAxis();
        Debug.Log($"vel post vel axis update {velocity}");
        if (desiredJump)
        {
            desiredJump = false;
            Jump(gravity);
        }
        else if (Climbing)
        {
            velocity -= contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
        }
        else if (InWater)
        {
            velocity += gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
        }
        else if (OnGround)
        {
            velocity = MovementMath.GetFloatingSpringVelocity(body, upAxis, downRayHit, velocity,
                rideHeight, rideSpringStrength, rideSpringDamper, Time.deltaTime);
        }
        else if (desiresClimbing && OnGround)
        {
            velocity += (gravity - contactNormal * (maxClimbAcceleration * 0.9f)) * Time.deltaTime;
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
        Debug.Log($"vel post update {velocity}");
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
    void ApplyVelocityAxis()
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
            jumpPhase = 0;
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
    void ApplySpringFloatingForce(Vector3 gravity)
    {
        velocity += gravity * Time.deltaTime;
        float vertVel = Vector3.Dot(-upAxis, velocity);
        float vertOtherVel = Vector3.Dot(-upAxis, connectionVelocity);

        float relVel = vertVel - vertOtherVel;

        //float x = rayHit.distance - rideHeight;
        //float springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);
        //Debug.Log($"sp{-upAxis * (springForce * Time.fixedDeltaTime / body.mass)}");
        //velocity += -upAxis * (springForce * Time.fixedDeltaTime / body.mass);
    }
    bool SnapToGround()
    {
        return false; //placeholder for testing
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2 || InWater)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        //if (!Physics.Raycast
        //    (
        //        body.position, -upAxis, out RaycastHit hit,
        //        probeDistance, probeMask, QueryTriggerInteraction.Ignore
        //    ))
        //{
        //    return false;
        //}
        //float upDot = Vector3.Dot(upAxis, hit.normal);
        //if (upDot < GetMinDot(hit.collider.gameObject.layer))
        //{
        //    return false;
        //}
        //groundContactCount = 1;
        //contactNormal = hit.normal;

        //float dot = Vector3.Dot(velocity, hit.normal);
        //if (dot > 0f)
        //{
        //    velocity = (velocity - hit.normal * dot).normalized * speed;
        //}
        //connectedBody = hit.rigidbody;
        //return true;
    }
    bool CheckSteepContacts()
    {
        return false; //placeholder for testing
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
        return false; //placeholder for testing
        if (Swimming)
        {
            groundContactCount = 0;
            contactNormal = upAxis;
            return true;
        }
        return false;
    }
    // prob not gonna be used
    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }

    //void Subscribe()
    //{
    //    UnSubscribe();
    //    player.OnCollisionEnterEvent += CollisionEnter;
    //    player.OnCollisionStayEvent += CollisionStay;
    //    player.OnTriggerEnterEvent += TriggerEnter;
    //    player.OnTriggerStayEvent += TriggerStay;
    //}

    //void UnSubscribe()
    //{
    //    player.OnCollisionEnterEvent -= CollisionEnter;
    //    player.OnCollisionStayEvent -= CollisionStay;
    //    player.OnTriggerEnterEvent -= TriggerEnter;
    //    player.OnTriggerStayEvent -= TriggerStay;
    //}
    //void CollisionEnter(Collision collision)
    //{
    //    EvaluateCollision(collision);
    //}

    //void CollisionStay(Collision collision)
    //{
    //    EvaluateCollision(collision);
    //}
    //void TriggerEnter(Collider other)
    //{
    //    if ((waterMask & (1 << other.gameObject.layer)) != 0)
    //    {
    //        EvaluateSubmergence(other);
    //    }
    //}
    //void TriggerStay(Collider other)
    //{
    //    if ((waterMask & (1 << other.gameObject.layer)) != 0)
    //    {
    //        EvaluateSubmergence(other);
    //    }
    //}

    //void EvaluateCollision(Collision collision)
    //{
    //    if (Swimming)
    //    {
    //        return;
    //    }
    //    int layer = collision.gameObject.layer;
    //    float minDot = GetMinDot(layer);
    //    for (int i = 0; i < collision.contactCount; i++)
    //    {
    //        Vector3 normal = collision.GetContact(i).normal;
    //        float upDot = Vector3.Dot(upAxis, normal);
    //        if (upDot >= minDot)
    //        {
    //            groundContactCount++;
    //            contactNormal += normal;
    //            connectedBody = collision.rigidbody;
    //        }
    //        else
    //        {
    //            if (upDot > -0.01f)
    //            {
    //                steepContactCount++;
    //                steepNormal += normal;
    //                if (groundContactCount == 0)
    //                {
    //                    connectedBody = collision.rigidbody;
    //                }
    //            }
    //            if (desiresClimbing && upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0)
    //            {
    //                climbContactCount++;

    //                climbNormal += normal;
    //                lastClimbNormal = normal;
    //                connectedBody = collision.rigidbody;
    //            }
    //        }
    //    }
    //}
    //void EvaluateRaycast(RaycastHit hit)
    //{
    //    if (Swimming)
    //    {
    //        return;
    //    }
    //    int layer = hit.collider.gameObject.layer;
    //    float minDot = GetMinDot(layer);
    //    Vector3 normal = hit.normal;
    //    float upDot = Vector3.Dot(upAxis, normal);
    //    if (upDot >= minDot)
    //    {
    //        groundContactCount++;
    //        contactNormal += normal;
    //        if (hit.rigidbody)
    //        {
    //            connectedBody = hit.rigidbody;
    //        }
    //    }
    //    else
    //    {
    //        if (upDot > -0.01f)
    //        {
    //            steepContactCount++;
    //            steepNormal += normal;
    //            if (groundContactCount == 0)
    //            {
    //                connectedBody = hit.rigidbody;
    //            }
    //        }
    //        if (desiresClimbing && upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0)
    //        {
    //            climbContactCount++;

    //            climbNormal += normal;
    //            lastClimbNormal = normal;
    //            if (hit.rigidbody)
    //            {
    //                connectedBody = hit.rigidbody;
    //            }
    //        }
    //    }
    //}
    //void EvaluateSubmergence(Collider collider)
    //{
    //    if (Physics.Raycast
    //    (
    //        body.position + upAxis * submergenceOffset,
    //        -upAxis, out RaycastHit hit, submergenceRange + 1f,
    //        waterMask, QueryTriggerInteraction.Collide
    //    ))
    //    {
    //        submergence = 1f - hit.distance / submergenceRange;
    //    }
    //    else
    //    {
    //        submergence = 1f;
    //    }
    //    if (Swimming)
    //    {
    //        connectedBody = collider.attachedRigidbody;
    //    }
    //}
    public override void LateUpdate() { }
    public void UpdatePlayerStatusAndContextValues()
    {
        player.Status.OnGround = OnGround;
        player.Status.OnSteep = OnSteep;
        player.Status.Climbing = Climbing;
        player.Status.InWater = InWater;
        player.Status.Swimming = Swimming;
        player.Status.Submergence = submergence;

        player.Status.StepsSinceLastGrounded = stepsSinceLastGrounded;

        player.ContactStatus.ConnectedBody = connectedBody;
        player.ContactStatus.PreviousConnectedBody = previousConnectedBody;
        player.ContactStatus.LastConnectionVelocity = lastConnectionVelocity;
        player.ContactStatus.LastContactNormal = lastContactNormal;
        player.ContactStatus.LastSteepNormal = lastSteepNormal;
    }
    public override void AssignConfigValues(PlayerController controller)
    {
        player = controller;
        config = player.NewSpringDrivenConfigSO;

        maxAcceleration = config.maxAcceleration;
        maxAirAcceleration = config.maxAirAcceleration;
        maxClimbAcceleration = config.maxClimbAcceleration;
        maxSwimAcceleration = config.maxSwimAcceleration;

        rideHeight = config.rideHeight;
        rideSpringStrength = config.rideSpringStrength;
        rideSpringDamper = config.rideSpringDamper;

        maxSpeed = config.maxSpeed;
        maxClimbSpeed = config.maxClimbSpeed;
        maxSwimSpeed = config.maxSwimSpeed;

        jumpHeight = config.jumpHeight;
        maxAirJumps = config.maxAirJumps;

        maxClimbAngle = config.maxClimbAngle;

        maxSnapSpeed = config.maxSnapSpeed;
        probeMask = config.probeMask;
        stairsMask = config.stairsMask;
        climbMask = config.climbMask;
        waterMask = config.waterMask;
        downRayDistance = config.downRayDistance;
        fwdRayDistance = config.fwdRayDistance;
        downBoxDistance = config.downBoxDistance;
        fwdRayDistance = config.fwdBoxDistance;
        downHalfExtents = config.downHalfExtents;
        fwdHalfExtents = config.fwdHalfExtents;

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
    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(raycastOrigin.position, sphereCastRadius);
    //    Gizmos.DrawSphere(raycastOrigin.position - upAxis * probeDistance, sphereCastRadius);
    //}
}
