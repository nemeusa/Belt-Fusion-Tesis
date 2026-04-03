using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float alturaMaxima = 10f;     // A qué Y llega
    public float alturaMinima = 0f;     // A qué Y vuelve
    public float velocidad = 2f;        // Qué tan rápido sube/baja

    [Header("Tiempos de Espera")]
    public float tiempoArriba = 3f;     // Cuánto se queda arriba
    public float tiempoAbajo = 5f;      // Cuánto tarda en volver a subir

    private Vector3 posMin;
    private Vector3 posMax;

    void Start()
    {
        // Guardamos las posiciones exactas
        posMin = transform.position;
        posMax = new Vector3(transform.position.x, alturaMaxima, transform.position.z);

        // Empezamos el ciclo
        StartCoroutine(CicloLava());
    }

    IEnumerator CicloLava()
    {
        while (true) // Bucle infinito para que no pare nunca
        {
            // 1. SUBIR
            yield return MoverLava(posMax);

            // 2. ESPERAR ARRIBA
            yield return new WaitForSeconds(tiempoArriba);

            // 3. BAJAR
            yield return MoverLava(posMin);

            // 4. ESPERAR ABAJO
            yield return new WaitForSeconds(tiempoAbajo);
        }
    }

    IEnumerator MoverLava(Vector3 destino)
    {
        // Mientras no hayamos llegado al destino...
        while (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null; // Espera al siguiente frame
        }

        // Aseguramos la posición final exacta
        transform.position = destino;
    }
}
