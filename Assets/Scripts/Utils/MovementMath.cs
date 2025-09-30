using UnityEngine;

public static class MovementMath
{
    public static void UpdateFloatingSpringPosition(OldPlayerController player,
        float RideHeight, float RideSpringStrength, float RideSpringDamper)
    {
        bool _rayDidHit = player.Raycasts.didRaycastHitDown;
        Vector3 DownDir = player.Raycasts.DownDir;

        Rigidbody _RB = player._rigidbody;

        if (_rayDidHit)
        {
            RaycastHit _rayHit = player.Raycasts.rayCastHitDown;
            Vector3 vel = _RB.linearVelocity;
            Vector3 rayDir = DownDir; //this should be =to forcefield

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

            //Debug.DrawLine(_RB.transform.position, _RB.transform.position + (rayDir * springForce), Color.yellow);

            player._rigidbody.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }
    }
    public static Vector3 GetFloatingSpringVelocity(Rigidbody body, Vector3 upAxis, RaycastHit hit,
        Vector3 velocity, float RideHeight, float RideSpringStrength, float RideSpringDamper, float timeStep)
    {
        // to test
        //Vector3 vel = velocity;  // option A
        //Vector3 vel = body.linearVelocity;  // option B
        Vector3 rayDir = -upAxis; //this should be =to forcefield

        Vector3 otherVel = Vector3.zero;
        Rigidbody hitBody = hit.rigidbody;
        if (hitBody)
        {
            otherVel = hitBody.linearVelocity;
        }

        float rayDirVel = Vector3.Dot(rayDir, velocity);
        float otherDirVel = Vector3.Dot(rayDir, otherVel);

        float relVel = rayDirVel - otherDirVel;

        float x = hit.distance - RideHeight;

        float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

        //Debug.DrawLine(_RB.transform.position, _RB.transform.position + (rayDir * springForce), Color.yellow);

        //body.AddForce(rayDir * springForce);
        Debug.Log($"{velocity}, new {velocity + rayDir * springForce / body.mass * timeStep}");
        velocity += rayDir * springForce/body.mass * timeStep;
        return velocity;
        //if (hitBody)
        //{
        //    hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
        //}
    }
}
