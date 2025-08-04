using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerController PlayerController;
    public virtual void TransitionIn() { OnEnable(); }
    public virtual void TransitionOut() { }
    public virtual void Act() { }
    public virtual void Interact() { }
    public virtual void Jump() { }
    public virtual void OnEnable()
    {
        Game.Input.OnJump += Jump;
        Game.Input.OnInteract += Interact;
    }
    public virtual void OnDisable()
    {
        Game.Input.OnJump -= Jump;
        Game.Input.OnInteract -= Interact;
    }

}
