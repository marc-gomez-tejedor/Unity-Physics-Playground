using UnityEngine;

public static class Game
{
    public static AllGravityBodies Gravity { get; private set; }

    public static void Init(AllGravityBodies gravity)
    {
        Gravity = gravity;
    }
}
