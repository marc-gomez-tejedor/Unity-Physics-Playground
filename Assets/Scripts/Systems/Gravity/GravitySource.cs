using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }
}
