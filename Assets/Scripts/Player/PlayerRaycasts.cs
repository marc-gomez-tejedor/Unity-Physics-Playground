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

    public (bool, RaycastHit) GetDownRaycastHit()
    {
        DownDir = -raycastOrigin.up;
        
        RaycastHit hit;
        if (Physics.Raycast(origin: raycastOrigin.position, direction: DownDir, out hit,
            maxDistance: maxDistanceThreshold, layerMask: groundLayerMask))
        {
            Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * hit.distance, Color.green);
            Debug.Log("Did Hit");
            return (true, hit);
        }
        else
        {
            Debug.DrawRay(start: raycastOrigin.position, dir: DownDir * maxDistanceThreshold, Color.red);
            Debug.Log("Did Hit");
            return (false, new RaycastHit());
        }
    }    
}
