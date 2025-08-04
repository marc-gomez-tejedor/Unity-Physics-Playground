using UnityEngine;

public class PlayerStateContainer : MonoBehaviour

{
    public PlayerState DefaultState;
    
    [Header("Idles")]
    public PlayerState Idle;

    [Header("Movement")]
    public PlayerState DefaultMove;
    public PlayerState MovingOnSpaceship;

    //[Header("OneTimeActions")] work on progress, for now its set in every state as an action
    //public PlayerState Jump;
}
