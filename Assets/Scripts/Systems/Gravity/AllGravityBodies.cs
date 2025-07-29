using System.Collections.Generic;
using UnityEngine;

public class AllGravityBodies
{
    public static List<Rigidbody> MassBodies { get; private set; }
    public static float NEWTONS_GRAVITATIONAL_CONSTANT { get; private set; } = 6f;
}
