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
}
