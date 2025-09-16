using UnityEngine;
using UnityEngine.Events;

public class AutomaticSlider : MonoBehaviour
{
    [SerializeField, Min(0.01f)]
    float duration = 1f;

    [SerializeField]
    UnityEvent<float> onValueChanged = default;

    float value;


    void FixedUpdate()
    {
        value += Time.deltaTime / duration;
        if (value >= 1f)
        {
            value = 1f;
            enabled = false;
        }
        onValueChanged.Invoke(value);
    }
}
