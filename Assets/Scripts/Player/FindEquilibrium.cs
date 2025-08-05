using UnityEngine;

public class FindEquilibrium : MonoBehaviour
{
    public Rigidbody _rigidbody;
    public void Center()
    {
        Vector3 torques = _rigidbody.GetAccumulatedTorque();
        // check floor with playercontroller.raycasts
        // aplly normal force vector in the direction of the floor normal
        // add torque to center rigidbody to that vector somehow
    }
}
