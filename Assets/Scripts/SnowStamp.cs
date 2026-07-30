using UnityEngine;

public class SnowStamp : MonoBehaviour
{
    [Header("Referencias")]
    public Texture2D stampTexture;

    [Header("Ajustes")]
    public float stampSize = 150f;

    SnowZone currentZone;
    Vector3 currentWorldPos;
    bool isOnSnow = false;

    float lastLogTime = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            currentZone = other.GetComponent<SnowZone>();
            isOnSnow = currentZone != null;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Snow"))
        {
            SnowZone zone = other.GetComponent<SnowZone>();
            if (zone != null)
            {
                currentZone = zone;
                isOnSnow = true;
                currentWorldPos = transform.position;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Snow"))
            isOnSnow = false;
    }

    void LateUpdate()
    {
        if (!isOnSnow || currentZone == null || currentZone.runtimeRT == null || stampTexture == null)
            return;

        StampAt(currentWorldPos);
    }

    void StampAt(Vector3 worldPos)
    {
        Vector3 localPos = currentZone.transform.InverseTransformPoint(worldPos);
        Bounds bounds = currentZone.mesh.bounds;

        float u = Mathf.InverseLerp(bounds.min.x, bounds.max.x, localPos.x);
        float v = Mathf.InverseLerp(bounds.min.z, bounds.max.z, localPos.z);

        if (Time.time - lastLogTime > 0.5f)
        {
            Debug.Log($"UV calculado: {u}, {v} | localPos: {localPos} | bounds min: {bounds.min} max: {bounds.max}");
            lastLogTime = Time.time;
        }

        RenderTexture rt = currentZone.runtimeRT;
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;

        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);

        Rect pixelRect = new Rect(
            u * rt.width - stampSize / 2f,
            v * rt.height - stampSize / 2f,
            stampSize,
            stampSize
        );

        Graphics.DrawTexture(pixelRect, stampTexture);

        GL.PopMatrix();
        RenderTexture.active = prev;
    }
}