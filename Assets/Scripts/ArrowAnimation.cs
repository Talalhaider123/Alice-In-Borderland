using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 2f;
    public float floatHeight = 0.25f;

    [Header("Scaling Settings")]
    public float scaleSpeed = 2f;
    public float scaleAmount = 0.1f;

    private Vector3 startPos;
    private Vector3 startScale;

    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    void Update()
    {
        // 🔽 Floating Up & Down
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, newY, 0);

        // 🔽 Scaling (Pulse effect)
        float scale = 1 + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
        transform.localScale = startScale * scale;
    }
}