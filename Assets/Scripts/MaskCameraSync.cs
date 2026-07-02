using Unity.Cinemachine;
using UnityEngine;

//[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class MaskCameraSync : MonoBehaviour
{
    private Camera mainCam;
    public Camera maskCam;
    private CinemachineBrain brain;

    void Start()
    {
        mainCam = transform.parent.GetComponent<Camera>();
        maskCam = GetComponent<Camera>();
        brain = transform.parent.GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        if (!TimeSlow.Instance.timeIsSlowed) return;

        if (brain != null && brain.UpdateMethod == CinemachineBrain.UpdateMethods.SmartUpdate)
        {
            Sync();
        }
    }

    //void LateUpdate()
    //{
    //    Sync();
    //}

    //void OnPreRender()
    //{
    //    Sync();
    //}

    void Sync()
    {
        transform.position = mainCam.transform.position;
        transform.rotation = mainCam.transform.rotation;
        maskCam.projectionMatrix = mainCam.projectionMatrix;
    }

    private void OnDisable()
    {
        maskCam.enabled = false;
    }
}
