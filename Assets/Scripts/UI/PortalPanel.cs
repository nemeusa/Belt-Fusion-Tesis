using UnityEngine;

public class PortalPanel : MonoBehaviour
{
    public PortalElement element = PortalElement.Neutral;
    public Transform worldRoot;   // lo que se mueve (portal + decorado 3D). NO el canvas
    public Transform portal;      // el portal que se atraviesa al salir de este panel
    public CanvasGroup ui;        // canvas con botones (opcional)

    [HideInInspector] public Vector3 restPos;
    bool _saved;

    [Tooltip("Dirección (en mundo) en la que avanza el decorado. Con tu offset (-34.2, 0, 40) es (0.65, 0, -0.76)")]
    public Vector3 travelDirection = new Vector3(0.65f, 0f, -0.76f);
    public Vector3 Axis => travelDirection.normalized;

    public bool alignCameraToPortal = true;

    [Header("Salir corriendo (opcional)")]
    public Transform[] runners;        // Player y robot. Vacío = todos solo rotan y avanzan con el portal
    public float runDistance = 40f;
    public float runSign = 1f;         // poné -1 si corren para el lado contrario

    [HideInInspector] public Quaternion restRot;

    public void SaveRest()
    {
        if (_saved) return;
        restPos = worldRoot.position;
        restRot = worldRoot.rotation;
        _saved = true;
    }

    void Awake() { SaveRest(); }
}