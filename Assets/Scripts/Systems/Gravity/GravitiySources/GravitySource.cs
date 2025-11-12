using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField]
    protected GravityType gravityType = GravityType.GravitySource;

    [Tooltip("If true, this gravity only affects when explicitly included.")]
    public virtual bool RequireExplicitInclude => false;
    public float scale = 9.8f;

    public GravityType GravityType => gravityType;

    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Vector3.down * scale;
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
