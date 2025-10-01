using UnityEngine;

public static class MathRaycasts
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
    public static bool GetBoxInfo(Vector3 origin, Vector3 direction, float rayDistance, 
        float boxDistance, Vector3 boxHalfExtents, LayerMask groundMask, out RaycastHit hit)
    {
        // First, try Raycast (precise)
        if (Physics.Raycast(origin, direction, out hit, rayDistance, groundMask))
        {
            Debug.Log("true ray");
            return true;
        }

        // Otherwise, try BoxCast (broader)
        // First lets orientate the pancake
        Quaternion orientation = Quaternion.FromToRotation(Vector3.up, direction);
        //Quaternion orientation = Quaternion.identity;
        if (Physics.BoxCast(origin, boxHalfExtents, direction, out hit,
            orientation, boxDistance, groundMask))
        {
            Debug.Log("true box");
            return true;
        }
        Debug.Log("false");
        return false; // No ground found
    }

}
