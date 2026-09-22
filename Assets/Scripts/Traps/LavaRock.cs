using DG.Tweening;
using UnityEngine;

public class LavaRock : MonoBehaviour
{
    [Header("Configuración de Hundimiento")]
    [SerializeField] private float distanciaHundimiento = 0.4f; // Cuánto baja en metros
    [SerializeField] private float tiempoBajada = 0.25f;        // Qué tan rápido se hunde al pisar
    [SerializeField] private float tiempoSubida = 0.5f;         // Qué tan rápido vuelve a subir

    [Header("Efectos de Suavizado (Easing)")]
    [SerializeField] private Ease curvaBajada = Ease.OutQuad;   // Bajada suave
    [SerializeField] private Ease curvaSubida = Ease.OutBack;   // Al subir hace un pequeño rebote natural

    private Vector3 posicionOriginal;
    private int entidadesSobrePiedra = 0;

    private void Start()
    {
        // Guardamos la posición inicial exacta de la piedra
        posicionOriginal = transform.localPosition;
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Si el objeto que piso la piedra tiene la etiqueta Player
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            entidadesSobrePiedra++;

            // Si es el primero en pisar, iniciamos el hundimiento
            if (entidadesSobrePiedra == 1)
            {
                Hundir();
            }
            Debug.Log("lo piso");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            entidadesSobrePiedra--;

            // Si el jugador saltó o se fue, la piedra vuelve a flotar
            if (entidadesSobrePiedra <= 0)
            {
                entidadesSobrePiedra = 0;
                VolverAFlotar();
            }
            Debug.Log("se fue xd");
        }
    }

    private void Hundir()
    {
        // Matamos cualquier animación previa de la piedra para que no haya tirones
        transform.DOKill();

        // Calculamos la posición hundida desde la posición original
        Vector3 posicionHundida = posicionOriginal - new Vector3(0, distanciaHundimiento, 0);

        // Animamos el movimiento vertical
        transform.DOLocalMove(posicionHundida, tiempoBajada).SetEase(curvaBajada);
    }

    private void VolverAFlotar()
    {
        transform.DOKill();

        // Volvemos a la posición inicial con efecto elástico
        transform.DOLocalMove(posicionOriginal, tiempoSubida).SetEase(curvaSubida);
    }
}
