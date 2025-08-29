using UnityEngine;

public class PlayerSetParent : MonoBehaviour
{
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
