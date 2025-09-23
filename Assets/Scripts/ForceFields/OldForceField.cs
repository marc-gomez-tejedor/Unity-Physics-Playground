using UnityEngine;

public class OldForceField : MonoBehaviour
{
    public Vector3 forceField;
    public float magnitude;

    public virtual Vector3 GetForceField()
    {
        return forceField;
    }
    public virtual Vector3 GetForceField(Transform target)
    {
        return forceField;
    }
}
