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
        UIText.text = player.Status.CurrentMoveState.ToString();
    }
}
