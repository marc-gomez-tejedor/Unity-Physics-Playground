using UnityEngine;

public class OldPlayerStateContainer : MonoBehaviour

{
    public OldPlayerState DefaultState;
    
    [Header("Idles")]
    public OldPlayerState Idle;

    [Header("Movement")]
    public OldPlayerState DefaultMove;
    public OldPlayerState MovingOnSpaceship;

    //[Header("OneTimeActions")] work in progress, for now its set in every state as an action
    //public PlayerState Jump;
}
