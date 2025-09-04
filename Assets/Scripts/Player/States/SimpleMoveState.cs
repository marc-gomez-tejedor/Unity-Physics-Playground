using UnityEngine;

public class SimpleMoveState : PlayerState
{
    [Header("General Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("spring parameters")]
    [SerializeField] private float RideHeight = 0.5f; // offset for floating spring
    [SerializeField] private float RideSpringStrength = 100f; // spring force
    [SerializeField] private float RideSpringDamper = 15f; // spring damping force
    
    public override void Act()
    {
        Vector2 inputDirection = Game.Input.MoveInput;
        Vector2 mouseInput = Game.Input.inputActions.Player.Look.ReadValue<Vector2>();
        Debug.Log($"inpL: {mouseInput}");
        Debug.Log($"inpM: {inputDirection}");
        PlayerController.FPVrotation.Move(mouseInput);

        /* -----------------------raycast----------------------------*/
        (bool, RaycastHit) ray = PlayerController.raycasts.GetDownRaycastHit();
        bool _rayDidHit = true;
        RaycastHit _rayHit = ray.Item2;
        Vector3 DownDir = -PlayerController.transform.up; //downwards raycast dir        
        /* ----------------------------------------------------------*/

        Rigidbody _RB = PlayerController._rigidbody;

        if (_rayDidHit)
        {
            Vector3 vel = _RB.linearVelocity;
            Vector3 rayDir = transform.TransformDirection(DownDir); //this should be =to forcefield
            rayDir = Vector3.down; //placeholder
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

            //Debug.DrawLine(_RB.transform.position, _RB.transform.position + (rayDir * springForce), Color.yellow);

            PlayerController._rigidbody.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }
    }
    public override void Jump()
    {
        PlayerController.movementBehaviour.Jump(jumpForce);
    }
}
