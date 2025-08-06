using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, IInitializable
{
    [Header("Interaction")]
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask interactLayerMask;

    public void Initialize()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        if (Game.Input != null) Game.Input.OnInteract += TryInteract;
    }

    private void OnDisable()
    {
        Game.Input.OnInteract -= TryInteract;
    }

    private void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayerMask);
        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log($"Interacting with {hit.name} -Interactor");
                interactable.Interact(this);
                break;
            }
        }
    }

    // For debugging
    private void OnDrawGizmosSelected()
    {
        if (!this.isActiveAndEnabled) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
