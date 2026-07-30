using UnityEngine;

public class SnowStamp : MonoBehaviour
{
    [Header("Referencias")]
    public RenderTexture snowRT;       // la RenderTexture "Interactive Snow"
    public Texture2D stampTexture;     // la textura snow_stamp (círculo)
    public Transform snowPlane;        // el GameObject "plano nieve"

    [Header("Ajustes")]
    public float stampSize = 150f;     // tamaño del sello en píxeles de la RT (ajustar a ojo)

    bool isOnSnow = false;
    Vector3 currentWorldPos;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            isOnSnow = true;
            currentWorldPos = transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = false;
    }

    void LateUpdate()
    {
        if (!isOnSnow || snowRT == null || stampTexture == null || snowPlane == null)
            return;

        StampAt(currentWorldPos);
    }

    void StampAt(Vector3 worldPos)
    {
        // Convertir posición world -> espacio local del plano
        Vector3 localPos = snowPlane.InverseTransformPoint(worldPos);

        MeshFilter mf = snowPlane.GetComponent<MeshFilter>();
        if (mf == null) return;

        Bounds bounds = mf.sharedMesh.bounds;

        // Mapear la posición local a UV (0 a 1) según los bounds del mesh
        float u = Mathf.InverseLerp(bounds.min.x, bounds.max.x, localPos.x);
        float v = Mathf.InverseLerp(bounds.min.z, bounds.max.z, localPos.z);

        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = snowRT;

        GL.PushMatrix();
        GL.LoadPixelMatrix(0, snowRT.width, snowRT.height, 0);

        Rect pixelRect = new Rect(
            u * snowRT.width - stampSize / 2f,
            v * snowRT.height - stampSize / 2f,
            stampSize,
            stampSize
        );

        Graphics.DrawTexture(pixelRect, stampTexture);

        GL.PopMatrix();
        RenderTexture.active = prev;
    }
}