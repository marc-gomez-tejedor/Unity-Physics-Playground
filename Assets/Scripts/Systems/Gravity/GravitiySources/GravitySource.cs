using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField]
    protected GravityType gravityType = GravityType.GravitySource;

    public GravityType GravityType => gravityType;

    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }
    void OnEnable()
    {
        CustomGravity.Register(this);
    }
    void OnDisable()
    {
        CustomGravity.Unregister(this);    
    }
}
