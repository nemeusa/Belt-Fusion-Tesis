using System.Collections;
using UnityEngine;

public class Whale : MonoBehaviour
{

    [SerializeField] private float alturaMaxima = 5f;    // Cuánto va a subir el chorro
    [SerializeField] private float velocidadWhaleSubeBaja = 3;
    [SerializeField] private float velocidadWaterSubeBaja = 8;

    [SerializeField] Transform waterPosition;

    private Vector3 waterDownPos;
    private Vector3 waterUpPos;

    [SerializeField] private float tiempoDisparando = 2f;  // Cuánto tiempo se queda arriba matando
    [SerializeField] private float tiempoOculto = 3f;

    private float currentWaterSpeed;
    private float currentWhaleSpeed;

    private Vector3 whaleNormalPos;
    private Vector3 whaleSubmergedPos;

    private void Start()
    {
        currentWaterSpeed = velocidadWaterSubeBaja;

        currentWhaleSpeed = velocidadWhaleSubeBaja;

        waterDownPos = waterPosition.position;

        waterUpPos = waterDownPos + new Vector3(0, alturaMaxima, 0);

        whaleNormalPos = transform.position; // Posición de disparo (arriba)
        whaleSubmergedPos = whaleNormalPos - new Vector3(0, 3, 0); // Posición sumergida

        StartCoroutine(CicloDelAgua());

    }


    private IEnumerator CicloDelAgua()
    {
        while (true)
        {
            // FASE 1: ESPERAR ABAJO (La ballena está sumergida tomando aire)
            yield return new WaitForSeconds(tiempoOculto);

            // FASE 2: SUBIR BALLENA (La ballena emerge a la superficie)
            while (Vector3.Distance(transform.position, whaleNormalPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, whaleNormalPos, currentWhaleSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = whaleNormalPos;

            // FASE 3: DISPARAR AGUA (El chorro sale disparado hacia arriba)
            while (Vector3.Distance(waterPosition.position, waterUpPos) > 0.01f)
            {
                waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterUpPos, currentWaterSpeed * Time.deltaTime);
                yield return null;
            }
            waterPosition.position = waterUpPos;

            // FASE 4: MANTENER DISPARO (Tiempo que el agua se queda bloqueando el camino)
            yield return new WaitForSeconds(tiempoDisparando);

            // FASE 5: BAJAR AGUA (El chorro se corta y vuelve a la boca)
            while (Vector3.Distance(waterPosition.position, waterDownPos) > 0.01f)
            {
                waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterDownPos, currentWaterSpeed * Time.deltaTime);
                yield return null;
            }
            waterPosition.position = waterDownPos;

            // FASE 6: SUMERGIR BALLENA (La ballena se vuelve a meter bajo el agua)
            while (Vector3.Distance(transform.position, whaleSubmergedPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, whaleSubmergedPos, currentWhaleSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = whaleSubmergedPos;
        }
    }

    //private IEnumerator CicloDelAgua()
    //{
    //    Vector3 v = new Vector3(0, transform.position.y, 0);
    //    Vector3 ven = new Vector3(0, transform.position.y - 5, 0);
    //    while (true)
    //    {

    //        //transform.position = v;
    //        // 1. ESPERAR ABAJO: La ballena está quieta tomando aire
    //        yield return new WaitForSeconds(tiempoOculto);

    //        //transform.position = transform.position + Vector3.up * 5;

    //        while (Vector3.Distance(new Vector3(0, transform.position.y, 0), v) > 0.01f)
    //        {
    //            transform.position = Vector3.MoveTowards(new Vector3(0, transform.position.y, 0), v, currentSpeed * Time.deltaTime);
    //            yield return null;
    //        }

    //        // 2. SUBIR: El chorro sube rápido hacia la altura máxima
    //        while (Vector3.Distance(waterPosition.position, waterUpPos) > 0.01f)
    //        {
    //            waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterUpPos, currentSpeed * Time.deltaTime);
    //            yield return null;
    //        }
    //        waterPosition.position = waterUpPos; // Aseguramos posición exacta arriba

    //        // 3. DISPARAR: Se queda arriba el tiempo que le digas tapando el camino
    //        yield return new WaitForSeconds(tiempoDisparando);

    //        // 4. BAJAR: El chorro se mete para adentro de golpe o suave
    //        while (Vector3.Distance(waterPosition.position, waterDownPos) > 0.01f)
    //        {
    //            waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterDownPos, currentSpeed * Time.deltaTime);
    //            yield return null;
    //        }
    //        waterPosition.position = waterDownPos; // Aseguramos posición exacta abajo

    //        while (Vector3.Distance(new Vector3(0, transform.position.y, 0), ven) > 0.01f)
    //        {
    //            waterPosition.position = Vector3.MoveTowards(new Vector3(0, transform.position.y, 0), ven, currentSpeed * Time.deltaTime);
    //            yield return null;
    //        }
    //    }
    //}

    private void RalentizarTrampa(float factor)
    {
        currentWaterSpeed = velocidadWaterSubeBaja * factor;
        currentWhaleSpeed = velocidadWhaleSubeBaja * factor;

    }

    private void NormalizarTrampa()
    {
        currentWaterSpeed = velocidadWaterSubeBaja;
        currentWhaleSpeed = velocidadWhaleSubeBaja;

    }


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
