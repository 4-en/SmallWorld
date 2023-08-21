using UnityEngine;

public class InvisibleShadowCaster : MonoBehaviour
{
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend)
        {
            rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            rend.material.SetInt("_ZWrite", 1);
            rend.material.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
            rend.material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Back);
            rend.material.SetInt("_RenderQueue", (int)UnityEngine.Rendering.RenderQueue.Transparent);
        }
    }
}