using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessFix : MonoBehaviour
{
    PostProcessLayer layer;

    void Start()
    {
        layer = GetComponent<PostProcessLayer>();

        if (layer != null)
        {
            layer.volumeLayer = LayerMask.GetMask("Default");
            layer.enabled = false;
            layer.enabled = true;
        }
    }
}