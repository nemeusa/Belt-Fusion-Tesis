using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class MaskCameraSync : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }

    void OnPreRender()
    {
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
    }
}
