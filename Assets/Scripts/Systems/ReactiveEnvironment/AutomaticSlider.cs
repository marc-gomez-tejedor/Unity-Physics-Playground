using UnityEngine;
using UnityEngine.Events;

public class AutomaticSlider : MonoBehaviour
{
    [SerializeField, Min(0.01f)]
    float duration = 1f;

    [SerializeField]
    bool autoReverse = false;

    [SerializeField]
    UnityEvent<float> onValueChanged = default;

    float value;

    bool reversed;


    void FixedUpdate()
    {
        float delta = Time.deltaTime / duration;
        if (reversed)
        {
            value -= delta;
            if (value <= 0f)
            {
                if (autoReverse)
                {
                    value = Mathf.Min(1f, -value);
                    reversed = false;
                }
                else
                {
                    value = 1f;
                    enabled = false;
                }
            }
        }
        else
        {
            value += delta;
            if (value >= 1f)
            {
                if (autoReverse)
                {
                    value = Mathf.Max(0f, 2f - value);
                    reversed = true;
                }
                else
                {
                    value = 1f;
                    enabled = false;
                }
            }
        }
        onValueChanged.Invoke(value);
    }
}
