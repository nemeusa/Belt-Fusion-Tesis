using UnityEngine;

public class SnowZone : MonoBehaviour
{
    [HideInInspector] public RenderTexture runtimeRT;
    [HideInInspector] public Mesh mesh;

    void Awake()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mf == null || mr == null)
        {
            Debug.LogWarning("SnowZone necesita MeshFilter y MeshRenderer en " + name);
            return;
        }

        mesh = mf.sharedMesh;

        // Crear una RenderTexture propia para esta zona (copiando el tamaño/formato de referencia)
        runtimeRT = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
        runtimeRT.wrapMode = TextureWrapMode.Clamp;
        runtimeRT.filterMode = FilterMode.Bilinear;
        runtimeRT.Create();

        // Limpiar la RT en negro al inicio
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = runtimeRT;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = prev;

        // Crear una instancia propia del material (para no compartirlo con otras zonas)
        Material instanceMat = new Material(mr.sharedMaterial);
        instanceMat.SetTexture("_RenderTexture", runtimeRT);
        mr.material = instanceMat; // "material" (no sharedMaterial) fuerza la instancia
    }
}