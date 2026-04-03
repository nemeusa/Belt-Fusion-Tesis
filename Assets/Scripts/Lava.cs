using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float alturaMaxima = 10f;    
    public float alturaMinima = 0f;   
    public float velocidad = 2f;      

    [Header("Tiempos de Espera")]
    public float tiempoArriba = 3f;   
    public float tiempoAbajo = 5f;     

    private Vector3 posMin;
    private Vector3 posMax;

    void Start()
    {
        posMin = transform.position;
        posMax = new Vector3(transform.position.x, alturaMaxima, transform.position.z);

        StartCoroutine(CicloLava());
    }

    IEnumerator CicloLava()
    {
        while (true) 
        {

            yield return MoverLava(posMax);


            yield return new WaitForSeconds(tiempoArriba);


            yield return MoverLava(posMin);


            yield return new WaitForSeconds(tiempoAbajo);
        }
    }

    IEnumerator MoverLava(Vector3 destino)
    {
        while (Vector3.Distance(transform.position, destino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
            yield return null; 
        }

        transform.position = destino;
    }
}
