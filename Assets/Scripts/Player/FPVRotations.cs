using Unity.VisualScripting;
using UnityEngine;

public class FPVRotations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraTransform;


    [Header("Parameters")]
    [SerializeField] private float speed;
    private float lookX = 0;
    private float lookY = 0;
    public void Move(Vector2 input)
    {
        lookX = input.x * speed * Time.deltaTime;
        lookY += input.y * speed * Time.deltaTime;

        lookY = Mathf.Clamp(lookY, -85f, 85f);

        playerTransform.localEulerAngles += new Vector3(0, lookX, 0);
        cameraTransform.localEulerAngles = new Vector3(-lookY, 0, 0);
    }
}
