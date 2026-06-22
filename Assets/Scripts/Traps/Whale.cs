using System.Collections;
using UnityEngine;

public class Whale : MonoBehaviour
{

    [SerializeField] private float alturaMaxima = 5f;    // Cuánto va a subir el chorro
    [SerializeField] private float velocidadSubeBaja = 8f;

    [SerializeField] Transform waterPosition;

    private Vector3 waterDownPos;
    private Vector3 waterUpPos;

    [SerializeField] private float tiempoDisparando = 2f;  // Cuánto tiempo se queda arriba matando
    [SerializeField] private float tiempoOculto = 3f;

    private float currentSpeed;


    private void Start()
    {
        currentSpeed = velocidadSubeBaja;

        waterDownPos = waterPosition.position;

        waterUpPos = waterDownPos + new Vector3(0, alturaMaxima, 0);

        StartCoroutine(CicloDelAgua());

    }


    private IEnumerator CicloDelAgua()
    {
        while (true)
        {

            transform.position = transform.position - Vector3.up * 5;
            // 1. ESPERAR ABAJO: La ballena está quieta tomando aire
            yield return new WaitForSeconds(tiempoOculto);

            transform.position = transform.position + Vector3.up * 5;
            // 2. SUBIR: El chorro sube rápido hacia la altura máxima
            while (Vector3.Distance(waterPosition.position, waterUpPos) > 0.01f)
            {
                waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterUpPos, currentSpeed * Time.deltaTime);
                yield return null;
            }
            waterPosition.position = waterUpPos; // Aseguramos posición exacta arriba

            // 3. DISPARAR: Se queda arriba el tiempo que le digas tapando el camino
            yield return new WaitForSeconds(tiempoDisparando);

            // 4. BAJAR: El chorro se mete para adentro de golpe o suave
            while (Vector3.Distance(waterPosition.position, waterDownPos) > 0.01f)
            {
                waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterDownPos, currentSpeed * Time.deltaTime);
                yield return null;
            }
            waterPosition.position = waterDownPos; // Aseguramos posición exacta abajo
        }
    }

    private void RalentizarTrampa(float factor) => currentSpeed = velocidadSubeBaja * factor;

    private void NormalizarTrampa() => currentSpeed = velocidadSubeBaja;


    private void OnEnable()
    {
        TimeSlow.OnTimeSlowed += RalentizarTrampa;
        TimeSlow.OnTimeNormalized += NormalizarTrampa;
    }

    private void OnDisable()
    {
        TimeSlow.OnTimeSlowed -= RalentizarTrampa;
        TimeSlow.OnTimeNormalized -= NormalizarTrampa;
    }
}
