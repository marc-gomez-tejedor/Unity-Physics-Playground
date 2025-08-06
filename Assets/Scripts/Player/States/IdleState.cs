using UnityEngine;

public class IdleState : PlayerState
{
    private void FixedUpdate()
    {
        base.PlayerController.findEquilibrium.Center();
    }
}
