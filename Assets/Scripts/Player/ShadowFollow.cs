using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    public Transform playerTransform; 
    public float offsetDelSuelo = 0.05f;
    public LayerMask capasSuelo;
    public float distanciaMaxima = 8f;

    public GameObject sombraNormal;                 // El círculo negro de siempre
    public GameObject sombraDisparable;             // El sprite que muestra que se puede disparar (ej: una mirilla)
    public LayerMask capaDisparable;            // La capa específica que detona el cambio de sprite

    void LateUpdate()
    {
        // 1. Escenario por defecto: vacío absoluto
        Vector3 posicionDestino = playerTransform.position + (Vector3.down * distanciaMaxima);
        float escalaDestino = 0.5f;

        // Asumimos por defecto que NO estamos sobre una zona disparable
        bool debeSerDisparable = false;

        RaycastHit hit;

        // 2. Tiramos el rayo
        if (Physics.Raycast(playerTransform.position, Vector3.down, out hit, 1000f, capasSuelo))
        {
            // 3. Verificamos si estamos dentro de la distancia máxima
            if (hit.distance <= distanciaMaxima)
            {
                posicionDestino = hit.point + (Vector3.up * offsetDelSuelo);
                escalaDestino = Mathf.Lerp(1.2f, 0.5f, hit.distance / distanciaMaxima);

                // 4. EL CÁLCULO DE LA CAPA: 
                if ((capaDisparable.value & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    // Confirmamos que SÍ tocamos la capa especial
                    debeSerDisparable = true;
                }
            }
        }

        // 5. Aplicamos todos los cambios de posición y escala a la sombra padre
        transform.position = posicionDestino;
        transform.localScale = new Vector3(escalaDestino, escalaDestino, escalaDestino);

        // 6. ACTIVAR / DESACTIVAR GAMEOBJECTS DE FORMA OPTIMIZADA
        // Solo hacemos el cambio si el estado actual no coincide con lo que debería ser
        if (sombraDisparable.activeSelf != debeSerDisparable)
        {
            sombraDisparable.SetActive(debeSerDisparable);
            sombraNormal.SetActive(!debeSerDisparable);
        }
    }
}
