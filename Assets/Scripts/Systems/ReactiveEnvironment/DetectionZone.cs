using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
public class DetectionZone : MonoBehaviour
{
    [SerializeField]
    UnityEvent onFirstEnter = default, onFirstExit = default;

    List<Collider> colliders = new List<Collider>(); 
    void OnTriggerEnter(Collider other)
    {
        if (colliders.Count == 0)
        {
            onFirstEnter.Invoke();
        }
        colliders.Add(other);
    }
    void OnTriggerExit(Collider other) 
    {
        if (colliders.Remove(other) && colliders.Count == 0)
        {
            onFirstExit.Invoke(); 
        }
    }
}
