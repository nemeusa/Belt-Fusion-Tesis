using UnityEngine;

public class InteractSphere : MonoBehaviour
{
    public float radio = 0.5f;
    public float distanciaMaxima = 10f;
    public LayerMask capaInteres; 

    void Update()
    {
        RaycastHit hit;

        // Origin, Radius, Direction, out HitInfo, MaxDistance, LayerMask
        if (Physics.SphereCast(transform.position, radio, transform.forward, out hit, distanciaMaxima, capaInteres))
        {
            Debug.Log("Detectado: " + hit.collider.name);

            // Puedes acceder al punto exacto de contacto
            Vector3 puntoContacto = hit.point;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 destino = transform.position + transform.forward * distanciaMaxima;

        // Dibuja el origen
        Gizmos.DrawWireSphere(transform.position, radio);
        // Dibuja el final
        Gizmos.DrawWireSphere(destino, radio);
        // Dibuja la línea de trayectoria
        Gizmos.DrawLine(transform.position, destino);
    }
}
