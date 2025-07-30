using System.Collections.Generic;
using UnityEngine;

public class AllGravityBodies : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> MassBodies;
    public static float NEWTONS_GRAVITATIONAL_CONSTANT { get; private set; } = 6.674f;
    public static float NEWTONS_GRAVITATIONAL_CONSTANT_EXP { get; private set; } = 4f;
    public List<Rigidbody> GetMassBodies()
    { 
        return MassBodies; 
    }
}
