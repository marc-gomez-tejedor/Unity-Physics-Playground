using UnityEngine;

public class SpringDemoState : PlayerState
{
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


    private void Awake()
    {
        baseColor = meshRenderer.material.color;
    }

    public override void UpdateInput()
    {
        float ratioColor = 0.5f + 0.5f*(1f - Mathf.Max(0f, (uprightSpringStrength-minForce)/(maxForce-minForce)));
        float ratioDamper = 0.2f + 0.8f*(Mathf.Max(0f, (uprightSpringDamper - minDamper)/(maxDamper - minDamper)));
        Color temp = baseColor * ratioColor;
        meshRenderer.material.color = new Color(temp.r, temp.g, temp.b, ratioDamper);
    }
    public override void Act()
    {
        PlayerController.Raycasts.UpdateRayCastDown();
        PlayerController.MovementBehaviour.UpdateFloatingSpringPosition(PlayerController, rideHeight, rideSpringStrength, rideSpringDamper);
        PlayerController.MovementBehaviour.UpdateUprightForce(PlayerController, uprightSpringStrength, uprightSpringDamper);
    }
}
