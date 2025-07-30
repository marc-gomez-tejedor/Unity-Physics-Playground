using System.Collections.Generic;
using UnityEngine;

public class AllGravityBodies : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> MassBodies;
    public static float NEWTONS_GRAVITATIONAL_CONSTANT { get; private set; } = 30f;
    public List<Rigidbody> GetMassBodies()
    { 
        return MassBodies; 
    }
}
