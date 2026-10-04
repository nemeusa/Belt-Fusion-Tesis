using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ShakeRock : MonoBehaviour
{

    public List<Transform> piedras = new List<Transform>();
    private bool paredRompida = false;

    [Header("Configuración del Temblor")]
    [Tooltip("Distancia máxima que se desplaza la piedra al temblar (valores chicos como 0.03 a 0.08 van muy bien)")]
    [SerializeField] private float intensidadPosicion = 0.04f;

    [Tooltip("Frecuencia de vibración por segundo")]
    [SerializeField] private int vibraciones = 25;

    [SerializeField] private float aleatoriedad = 90f;

    [Header("Opciones Extras")]
    [SerializeField] private bool incluirRotacion = true;
    [SerializeField] private float intensidadRotacion = 1.5f; // Grados de vibración en rotación

    private Tween tweenPosicion;
    private Tween tweenRotacion;

    [SerializeField] GameObject lavaObj;

    [Header("Configuración de Explosión con DOTween")]
    [SerializeField] private float distanciaVuelo = 7f;       // Qué tan lejos llegan las piedras hacia adelante
    [SerializeField] private float alturaArco = 2f;           // Altura de la parábola/salto en el aire
    [SerializeField] private float dispersion = 1.5f;         // Desviación aleatoria para que no vayan todas en fila
    [SerializeField] private float tiempoVuelo = 1f;          // Duración de la animación completa
    [SerializeField] private Ease curvaVuelo = Ease.OutQuad;


    private void Start()
    {

        IniciarTemblorPiedras();
    }

    private void IniciarTemblorPiedras()
    {
        // Limpiamos tweens previos por seguridad
        DetenerTemblor();

        foreach (Transform piedra in piedras)
        {

            tweenPosicion = piedra.DOShakePosition(1f, intensidadPosicion, vibraciones, aleatoriedad, false, false)
                                     .SetLoops(-1, LoopType.Restart);

            if (incluirRotacion)
            {
                tweenRotacion = piedra.DOShakeRotation(1f, intensidadRotacion, vibraciones, aleatoriedad, false)
                                         .SetLoops(-1, LoopType.Restart);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (paredRompida) return;

        // Si el objeto que toca la pared es el jugador
        if (other.GetComponent<PlayerController>() != null)
        {
            // Tomamos la dirección hacia donde miraba o se movía el player

            lavaObj.SetActive(false);
            Vector3 direccionEmpuje = other.transform.forward;
            RomperPared(direccionEmpuje);
        }
    }

    public void RomperPared(Vector3 direccionEmpuje)
    {
        paredRompida = true;

        // Desactivamos el collider del padre
        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        foreach (Transform piedra in piedras)
        {
            // 1. Frenamos el temblor previo
            piedra.DOKill();

            // 2. Calculamos el punto de destino proyectado hacia adelante con dispersión aleatoria
            Vector3 dispersionAleatoria = Random.insideUnitSphere * dispersion;
            Vector3 destinoFinal = piedra.position + (direccionEmpuje.normalized * distanciaVuelo) + dispersionAleatoria;

            // 3. Creamos una Secuencia para combinar arco, rotación y desaparición
            Sequence secuenciaPiedra = DOTween.Sequence();

            // DOJump hace la trayectoria de parábola/disparo hacia el destino
            secuenciaPiedra.Join(piedra.DOJump(destinoFinal, alturaArco, 1, tiempoVuelo).SetEase(curvaVuelo));

            // Rotación aleatoria en 360 grados mientras vuela
            Vector3 girosAleatorios = new Vector3(
                Random.Range(180f, 540f),
                Random.Range(180f, 540f),
                Random.Range(180f, 540f)
            );
            secuenciaPiedra.Join(piedra.DORotate(girosAleatorios, tiempoVuelo, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // achicamos la piedra en el último 30% del trayecto para que desaparezca suave
            secuenciaPiedra.Insert(tiempoVuelo * 0.7f, piedra.DOScale(Vector3.zero, tiempoVuelo * 0.3f).SetEase(Ease.InQuad));

            // Destruimos el GameObject al terminar su animación individual
            secuenciaPiedra.OnComplete(() =>
            {
                if (piedra != null) Destroy(piedra.gameObject);
            });
        }

        // Destruimos el objeto padre una vez que terminan las animaciones
        Destroy(gameObject, tiempoVuelo + 0.1f);
    }


    private void OnDestroy()
    {
        // Limpieza preventiva de Tweens
        foreach (Transform piedra in piedras)
        {
            if (piedra != null) piedra.DOKill();
        }
    }

 

    public void DetenerTemblor()
    {
        if (tweenPosicion != null && tweenPosicion.IsActive()) tweenPosicion.Kill();
        if (tweenRotacion != null && tweenRotacion.IsActive()) tweenRotacion.Kill();
    }


}
