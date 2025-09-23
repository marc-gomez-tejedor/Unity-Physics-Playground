using UnityEngine;

public class OldCentipetalFromMothership : OldForceField
{
    [SerializeField] private Transform mothershipTransform;
    public override Vector3 GetForceField(Transform target)
    {
        Vector3 rotationCenter = mothershipTransform.right;
        Vector3 center = new Vector3(target.position.x, mothershipTransform.position.y, mothershipTransform.position.z);
        //Vector3 plane = rotationCenter.
        return center;
    }
}
