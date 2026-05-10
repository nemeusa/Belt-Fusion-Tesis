using UnityEngine;

public class WallData : MonoBehaviour
{
    // Aquí ponés la dirección (ej: 1,0,0 para X positivo)
    // Podés usar las flechitas en el Inspector para orientarlo
    public Vector3 runDirection = Vector3.forward;

    private void OnDrawGizmos()
    {
        // Dibujamos una flecha azul para ver la dirección en el editor
        Gizmos.color = Color.blue;
        Vector3 start = transform.position + Vector3.up;
        Gizmos.DrawRay(start, transform.TransformDirection(runDirection) * 2f);
    }
}
