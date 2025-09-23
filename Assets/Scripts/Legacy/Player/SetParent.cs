using UnityEngine;

public class SetParent : MonoBehaviour
{
    public void SetTo(Transform parent)
    {
        transform.parent = parent;
    }
    public void Unset()
    {
        transform.parent = null;
    }
}
