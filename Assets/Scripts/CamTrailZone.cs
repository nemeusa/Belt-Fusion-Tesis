using Unity.Cinemachine;
using UnityEngine;

public class CamTrailZone : MonoBehaviour
{
    public CinemachineCamera vCamTarget; 
    public int highPriority = 20;
    public int lowPriority = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            // Al darle más prioridad, Cinemachine hace el "blend" (transición) sola
            vCamTarget.Priority = highPriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().is2Dmoving = false;
            // Al bajar la prioridad, vuelve a la cámara anterior
        }
    }
}
