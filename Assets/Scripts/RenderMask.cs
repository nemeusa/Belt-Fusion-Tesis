using UnityEngine;

[ExecuteInEditMode]
public class RenderMask : MonoBehaviour
{
    public Material postMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (postMaterial != null)
            Graphics.Blit(src, dest, postMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
