using TMPro;
using UnityEngine;

public class PlayerStatusLogger : MonoBehaviour
{
    [SerializeField]
    PlayerController player;
    [SerializeField]
    TMP_Text UIText;

    void Update()
    {
        enumState currentPlayerState = player.Status.CurrentAttackState;
        if (currentPlayerState == enumState.None)
        {
            currentPlayerState = player.Status.CurrentMoveState;
        }
        UIText.text = currentPlayerState.ToString();
    }
}
