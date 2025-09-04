using Unity.VisualScripting;
using UnityEngine;

public class FPVRotations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;


    [Header("Parameters")]
    [SerializeField] private float speed;
    private Vector2 look = Vector2.zero;
    public void Move(Vector2 input)
    {
        look += input * speed;

        look.y = Mathf.Clamp(look.y, -85f, 85f);

        playerTransform.localEulerAngles = new Vector3(0, look.x, 0);
        cameraTransform.localEulerAngles = new Vector3(-look.y, 0, 0);
        Debug.Log($"look: {look}");

    }
}
