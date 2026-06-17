using System.Collections;
using UnityEngine;

public class ShotWaterBallena : MonoBehaviour
{


    [SerializeField] AudioClip deathAudio;
    [SerializeField] float deathDuration = 0.2f;

    [SerializeField] private float alturaMaxima = 5f;    // Cuánto va a subir el chorro
    [SerializeField] private float velocidadSubeBaja = 8f;

    [SerializeField] float speedOutCamera = 10;

    [SerializeField, Range(0, 1)] float _icePlayerForceWater = 1;

    bool isFlyDeath;

    Transform playerMesh;

    private Vector3 posicionOculto;
    private Vector3 posicionDisparo;

    [SerializeField] private float tiempoDisparando = 2f;  // Cuánto tiempo se queda arriba matando
    [SerializeField] private float tiempoOculto = 3f;

    private float currentSpeed;


    private void Start()
    {
        currentSpeed = velocidadSubeBaja;

        posicionOculto = transform.position;

        posicionDisparo = posicionOculto + new Vector3(0, alturaMaxima, 0);

        StartCoroutine(CicloDelAgua());

    }

    private void LateUpdate()
    {
        if (isFlyDeath) playerMesh.position += Vector3.up * speedOutCamera * Time.deltaTime;

    }



    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            if (!player._fsm.WhatCurrentState(TypeFSM.Ice))
            {
                playerMesh = player.meshFather.transform;

                StartCoroutine(OutCamDeath());

                GameManager.instance.Death(collision.gameObject, deathDuration, deathAudio, null);

            }

            else player._playerVelocity.y += _icePlayerForceWater;

        }
    }


    private IEnumerator CicloDelAgua()
    {
        while (true)
        {
            // 1. ESPERAR ABAJO: La ballena está quieta tomando aire
            yield return new WaitForSeconds(tiempoOculto);

            // 2. SUBIR: El chorro sube rápido hacia la altura máxima
            while (Vector3.Distance(transform.position, posicionDisparo) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionDisparo, currentSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = posicionDisparo; // Aseguramos posición exacta arriba

            // 3. DISPARAR: Se queda arriba el tiempo que le digas tapando el camino
            yield return new WaitForSeconds(tiempoDisparando);

            // 4. BAJAR: El chorro se mete para adentro de golpe o suave
            while (Vector3.Distance(transform.position, posicionOculto) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, posicionOculto, currentSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = posicionOculto; // Aseguramos posición exacta abajo
        }
    }
    IEnumerator OutCamDeath()
    {
        isFlyDeath = true;

        yield return new WaitForSeconds(deathDuration);

        isFlyDeath = false;

        playerMesh = null;
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
