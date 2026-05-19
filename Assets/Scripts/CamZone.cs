using Unity.Cinemachine;
using UnityEngine;


public class CamZone : MonoBehaviour
{
    public CinemachineCamera vCamTarget; // Arrastrá la cámara 2D aquí
    public int highPriority = 20;
    public int lowPriority = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            // Al darle más prioridad, Cinemachine hace el "blend" (transición) sola
            other.GetComponent<PlayerController>().is2Dmoving = true;
            vCamTarget.Priority = highPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().is2Dmoving = false;
            // Al bajar la prioridad, vuelve a la cámara anterior
            vCamTarget.Priority = lowPriority;
        }
    }
}
