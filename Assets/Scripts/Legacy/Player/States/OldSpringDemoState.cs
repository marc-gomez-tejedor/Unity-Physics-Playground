using UnityEngine;
using UnityEngine.UIElements;

public class OldSpringDemoState : OldPlayerState
{
    [SerializeField]
    Rigidbody rb;

    [Header("Floating spring parameters")]
    [SerializeField] private float rideHeight = 0.5f; // offset for floating spring
    [SerializeField] private float rideSpringStrength = 100f; // spring force
    [SerializeField] private float rideSpringDamper = 15f; // spring damping force

    [Header("Upright spring parameters")]
    [SerializeField] private float uprightSpringStrength = 100f; // upright spring force
    [SerializeField] private float uprightSpringDamper = 15f; // upright spring damping force

    private float minForce = 10f, maxForce = 1000f;
    private float minDamper = 1f, maxDamper = 40f;

    [SerializeField]
    MeshRenderer meshRenderer;
    Color baseColor;

    bool move = false, interact = false;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        baseColor = meshRenderer.material.color;
    }

    public override void UpdateInput()
    {
        float ratioColor = 0.5f + 0.5f*(1f - Mathf.Max(0f, (uprightSpringStrength-minForce)/(maxForce-minForce)));
        float ratioDamper = 0.2f + 0.8f*(Mathf.Max(0f, (uprightSpringDamper - minDamper)/(maxDamper - minDamper)));
        Color temp = baseColor * ratioColor;
        meshRenderer.material.color = new Color(temp.r, temp.g, temp.b, ratioDamper);
        if (Input.GetButtonDown("Interact"))
        {
            interact = true;
        }
    }
    public override void Act()
    {
        if (move)
        {
            rb.AddTorque(0f, 0f, -30f, ForceMode.Impulse);
            move = false;
        }
        if (interact && !move)
        {
            move = true;
            interact = false;
            rb.angularVelocity = Vector3.zero;
            rb.transform.rotation = Quaternion.identity;
            return;
        }
        PlayerController.Raycasts.UpdateRayCastDown();
        PlayerController.MovementBehaviour.UpdateFloatingSpringPosition(PlayerController, rideHeight, rideSpringStrength, rideSpringDamper);
        PlayerController.MovementBehaviour.UpdateUprightForce(PlayerController, uprightSpringStrength, uprightSpringDamper);
    }
}
