using System.Collections;
using UnityEngine;

public class Whale : TimeTrap
{
    [SerializeField] private float alturaMaxima = 5f; 
    [SerializeField] private float velocidadWhaleSubeBaja = 3;
    [SerializeField] private float velocidadWaterSubeBaja = 8;

    [SerializeField] Transform waterPosition;

    private Vector3 waterDownPos;
    private Vector3 waterUpPos;

    [SerializeField] private float tiempoDisparando = 2f; 
    [SerializeField] private float tiempoOculto = 3f;

    private float currentWaterSpeed;
    private float currentWhaleSpeed;
    private float currentWhaleOcultTime;

    private Vector3 whaleNormalPos;
    private Vector3 whaleSubmergedPos;

    [SerializeField] Animator waterAni;

    private void Start()
    {
        currentWhaleOcultTime = tiempoOculto;
        currentWaterSpeed = velocidadWaterSubeBaja;
        currentWhaleSpeed = velocidadWhaleSubeBaja;

        waterDownPos = waterPosition.position;

        waterUpPos = waterDownPos + new Vector3(0, alturaMaxima, 0);

        whaleNormalPos = transform.position;
        whaleSubmergedPos = whaleNormalPos - new Vector3(0, 3, 0);

        StartCoroutine(CicloDelAgua());

    }


    private IEnumerator CicloDelAgua()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentWhaleOcultTime);

            //while (Vector3.Distance(transform.position, whaleNormalPos) > 0.01f)
            //{
            //    transform.position = Vector3.MoveTowards(transform.position, whaleNormalPos, currentWhaleSpeed * Time.deltaTime);
            //    yield return null;
            //}
            //transform.position = whaleNormalPos;

            //while (Vector3.Distance(waterPosition.position, waterUpPos) > 0.01f)
            //{
            //    waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterUpPos, currentWaterSpeed * Time.deltaTime);
            //    yield return null;
            //}
            //waterPosition.position = waterUpPos;

            waterAni.SetBool("Up", true);
            yield return new WaitForSeconds(0.50f);

            yield return new WaitForSeconds(tiempoDisparando);

            //while (Vector3.Distance(waterPosition.position, waterDownPos) > 0.01f)
            //{
            //    waterPosition.position = Vector3.MoveTowards(waterPosition.position, waterDownPos, currentWaterSpeed * Time.deltaTime);
            //    yield return null;
            //}
            //waterPosition.position = waterDownPos;
            waterAni.SetBool("Up", false);
            yield return new WaitForSeconds(0.50f);

            //while (Vector3.Distance(transform.position, whaleSubmergedPos) > 0.01f)
            //{
            //    transform.position = Vector3.MoveTowards(transform.position, whaleSubmergedPos, currentWhaleSpeed * Time.deltaTime);
            //    yield return null;
            //}
            //transform.position = whaleSubmergedPos;
        }
    }

    protected override void SlowdownTrap(float factor)
    {
        waterAni.speed = 1 * factor;
        currentWaterSpeed = velocidadWaterSubeBaja * factor;
        currentWhaleSpeed = velocidadWhaleSubeBaja * factor;
        //currentWhaleOcultTime = tiempoOculto * factor;
    }

    protected override void NormalizeTrap()
    {
        waterAni.speed = 1;
        currentWaterSpeed = velocidadWaterSubeBaja;
        currentWhaleSpeed = velocidadWhaleSubeBaja;
        //currentWhaleOcultTime = tiempoOculto;
    }
}
