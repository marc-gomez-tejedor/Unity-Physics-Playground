using UnityEngine;

public class OldOnCollisionController : MonoBehaviour
{
    public bool onGround {  get; private set; }
    public Vector3 groundNormal {  get; private set; }
    [SerializeField] private OldPlayerController player;

    public void TurnToFalse()
    {
        onGround = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        /*GameObject currentGround = null;
        if (player.Raycasts.didRaycastHitDown) 
        {
            currentGround = player.Raycasts.rayCastHitDown.collider.gameObject;
        }
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.gameObject == currentGround)
            {
                onGround = true;
                groundNormal = collision.GetContact(i).normal;
                Debug.Log($"onground: {onGround}, normal:{groundNormal}");
            }
        }
        */
    }
}
