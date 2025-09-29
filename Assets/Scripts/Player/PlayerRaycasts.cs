using UnityEngine;

public static class Raycasts
{
    public static void CastRay(Vector3 Origin, Vector3 Direction,
        ref bool didRaycastHitDown, ref RaycastHit hit,
        float maxDistanceThreshold, LayerMask mask)
    {
        if (Physics.Raycast(origin: Origin, direction: Direction, out hit,
            maxDistance: maxDistanceThreshold, layerMask: mask))
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * hit.distance, Color.green);
            //Debug.Log($"Did Hit: {hit.normal}");
            didRaycastHitDown = true;
        }
        else
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * maxDistanceThreshold, Color.red);
            //Debug.Log("Did Hit");
            didRaycastHitDown = false;
            hit = new RaycastHit();
        }
    }
    public static void CastSphere(Vector3 Origin, float Radius, Vector3 Direction,
        ref bool didRaycastHitDown, ref RaycastHit hit,
        float maxDistanceThreshold, LayerMask mask)
    {
        if (Physics.SphereCast(origin: Origin, radius:Radius, direction: Direction, out hit,
            maxDistance: maxDistanceThreshold, layerMask: mask))
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * hit.distance, Color.green);
            //Debug.Log($"Did Hit: {hit.normal}");
            didRaycastHitDown = true;
        }
        else
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * maxDistanceThreshold, Color.red);
            //Debug.Log("Did Hit");
            didRaycastHitDown = false;
            hit = new RaycastHit();
        }
    }
}
