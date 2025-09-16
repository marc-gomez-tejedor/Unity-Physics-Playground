using UnityEngine;
using UnityEngine.Events;
public class DetectionZone : MonoBehaviour
{
    [SerializeField]
    UnityEvent onEnter = default, onExit = default;

    void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke();    
    }
    void OnTriggerExit(Collider other) 
    { 
        onExit.Invoke(); 
    }
}
