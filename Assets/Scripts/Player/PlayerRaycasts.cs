using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRaycasts : MonoBehaviour
{
    [Header("General Parameters")]
    public Vector3 DownDir { get; private set; }
    [SerializeField] private float maxDistanceThreshold = 1f;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform raycastOrigin;
    public bool didRaycastHitDown { get; private set; } = false;
    public RaycastHit rayCastHitDown { get; private set; }

    public void UpdateRayCastDown()
    {
        DownDir = -raycastOrigin.up;

        RaycastHit hit;
        if (Physics.Raycast(origin: raycastOrigin.position, direction: DownDir, out hit,
            maxDistance: maxDistanceThreshold, layerMask: groundLayerMask))
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * hit.distance, Color.green);
            Debug.Log($"Did Hit: {hit.normal}");
            didRaycastHitDown = true;
            rayCastHitDown = hit;
        }
        else
        {
            //Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * maxDistanceThreshold, Color.red);
            //Debug.Log("Did Hit");
            didRaycastHitDown = false;
            rayCastHitDown = new RaycastHit();
        }
    }
}
