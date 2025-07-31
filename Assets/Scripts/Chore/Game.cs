using UnityEngine;

public static class Game
{
    public static AllGravityBodies Gravity { get; private set; }
    public static InputManager Input { get; private set; }

    public static void Init(AllGravityBodies gravity, InputManager input)
    {
        Gravity = gravity;
        Input = input;
    }
}
