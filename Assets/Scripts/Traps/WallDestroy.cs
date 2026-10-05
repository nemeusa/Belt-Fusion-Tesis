using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroy : MonoBehaviour, DashInteract
{

    [SerializeField] List<Transform> piedras = new List<Transform>();
    [SerializeField] GameObject lavaObj;
    [SerializeField] GameObject desObj;

    // Listas para recordar el estado inicial completo
    private List<Vector3> piedrasInicialPocision = new List<Vector3>();
    private List<Quaternion> piedrasInicialRotacion = new List<Quaternion>();
    private List<Vector3> piedrasInicialEscala = new List<Vector3>();

    private bool paredRompida = false;
    private bool act;

    [Header("Configuración del Temblor")]
    [SerializeField] private float intensidadPosicion = 0.04f;
    [SerializeField] private int vibraciones = 25;
    [SerializeField] private float aleatoriedad = 90f;

    [Header("Opciones Extras")]
    [SerializeField] private bool incluirRotacion = true;
    [SerializeField] private float intensidadRotacion = 1.5f;

    [Header("Configuración de Explosión con DOTween")]
    [SerializeField] private float distanciaVuelo = 7f;
    [SerializeField] private float alturaArco = 2f;
    [SerializeField] private float dispersion = 1.5f;
    [SerializeField] private float tiempoVuelo = 1f;
    [SerializeField] private Ease curvaVuelo = Ease.OutQuad;

    private void Start()
    {
        // Guardamos posicion, rotacion y escala originales de cada piedra
        foreach (var p in piedras)
        {
            piedrasInicialPocision.Add(p.position);
            piedrasInicialRotacion.Add(p.rotation);
            piedrasInicialEscala.Add(p.localScale);
        }

        IniciarTemblorPiedras();
    }

    private void Update()
    {
        if (CheckpointManager.Instance.respawn)
        {
            ResetPared();
        }
    }

    public void ExcuteAction(Collider target)
    {
        if (paredRompida) return;


        if (lavaObj != null) lavaObj.SetActive(false);

        Vector3 direccionEmpuje = target.transform.forward;
        RomperPared(direccionEmpuje);

        act = false;

    }

    private void IniciarTemblorPiedras()
    {
        DetenerTemblor();

        foreach (Transform piedra in piedras)
        {
            piedra.DOShakePosition(1f, intensidadPosicion, vibraciones, aleatoriedad, false, false)
                  .SetLoops(-1, LoopType.Restart);

            if (incluirRotacion)
            {
                piedra.DOShakeRotation(1f, intensidadRotacion, vibraciones, aleatoriedad, false)
                      .SetLoops(-1, LoopType.Restart);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (paredRompida) return;

    //    if (other.GetComponent<PlayerController>() != null)
    //    {
    //        if (lavaObj != null) lavaObj.SetActive(false);

    //        Vector3 direccionEmpuje = other.transform.forward;
    //        RomperPared(direccionEmpuje);

    //        act = false;
    //    }
    //}

    public void RomperPared(Vector3 direccionEmpuje)
    {
        paredRompida = true;

        // Desactivamos el collider
        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        foreach (Transform piedra in piedras)
        {
            piedra.DOKill();

            Vector3 dispersionAleatoria = Random.insideUnitSphere * dispersion;
            Vector3 destinoFinal = piedra.position + (direccionEmpuje.normalized * distanciaVuelo) + dispersionAleatoria;

            Sequence secuenciaPiedra = DOTween.Sequence();

            secuenciaPiedra.Join(piedra.DOJump(destinoFinal, alturaArco, 1, tiempoVuelo).SetEase(curvaVuelo));

            Vector3 girosAleatorios = new Vector3(
                Random.Range(180f, 540f),
                Random.Range(180f, 540f),
                Random.Range(180f, 540f)
            );
            secuenciaPiedra.Join(piedra.DORotate(girosAleatorios, tiempoVuelo, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // Achicamos la piedra
            secuenciaPiedra.Insert(tiempoVuelo * 0.7f, piedra.DOScale(Vector3.zero, tiempoVuelo * 0.3f).SetEase(Ease.InQuad));
        }
    }

    public void ResetPared()
    {
        paredRompida = false;

        // Volvemos a activar objetos y collider
        if (desObj != null) desObj.SetActive(true);
        if (lavaObj != null) lavaObj.SetActive(true);
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = true;

        // Restauramos transformaciones de cada piedra
        for (int i = 0; i < piedras.Count; i++)
        {
            piedras[i].DOKill(); // Matamos animaciones anteriores de la piedra
            piedras[i].position = piedrasInicialPocision[i];
            piedras[i].rotation = piedrasInicialRotacion[i];
            piedras[i].localScale = piedrasInicialEscala[i]; // ¡Devolvemos la escala visible!
        }

        // Reiniciamos el temblor
        IniciarTemblorPiedras();
    }

    public void DetenerTemblor()
    {
        foreach (Transform piedra in piedras)
        {
            if (piedra != null) piedra.DOKill();
        }
    }

    private void OnDestroy()
    {
        DetenerTemblor();
    }


    //[SerializeField] List<Transform> piedras = new List<Transform>();
    //[SerializeField] GameObject lavaObj;
    //private List<Vector3> piedrasInicialPocision = new List<Vector3>();
    //private bool paredRompida = false;

    //[Header("Configuración del Temblor")]
    //[SerializeField] private float intensidadPosicion = 0.04f;
    //[SerializeField] private int vibraciones = 25;
    //[SerializeField] private float aleatoriedad = 90f;

    //[Header("Opciones Extras")]
    //[SerializeField] private bool incluirRotacion = true;
    //[SerializeField] private float intensidadRotacion = 1.5f; // Grados de vibración en rotación

    //private Tween tweenPosicion;
    //private Tween tweenRotacion;

    //[Header("Configuración de Explosión con DOTween")]
    //[SerializeField] private float distanciaVuelo = 7f;       // Qué tan lejos llegan las piedras hacia adelante
    //[SerializeField] private float alturaArco = 2f;           // Altura de la parábola/salto en el aire
    //[SerializeField] private float dispersion = 1.5f;         // Desviación aleatoria para que no vayan todas en fila
    //[SerializeField] private float tiempoVuelo = 1f;          // Duración de la animación completa
    //[SerializeField] private Ease curvaVuelo = Ease.OutQuad;


    //private void Start()
    //{
    //    foreach (var p in piedras)
    //    {
    //        piedrasInicialPocision.Add(p.position);
    //    }
    //    IniciarTemblorPiedras();
    //}

    //private void IniciarTemblorPiedras()
    //{
    //    // Limpiamos tweens previos por seguridad
    //    DetenerTemblor();

    //    foreach (Transform piedra in piedras)
    //    {

    //        tweenPosicion = piedra.DOShakePosition(1f, intensidadPosicion, vibraciones, aleatoriedad, false, false)
    //                                 .SetLoops(-1, LoopType.Restart);

    //        if (incluirRotacion)
    //        {
    //            tweenRotacion = piedra.DOShakeRotation(1f, intensidadRotacion, vibraciones, aleatoriedad, false)
    //                                     .SetLoops(-1, LoopType.Restart);
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (paredRompida) return;

    //    // Si el objeto que toca la pared es el jugador
    //    if (other.GetComponent<PlayerController>() != null)
    //    {
    //        // Tomamos la dirección hacia donde miraba o se movía el player

    //        lavaObj.SetActive(false);
    //        Vector3 direccionEmpuje = other.transform.forward;
    //        RomperPared(direccionEmpuje);
    //        //desObj.SetActive(false);
    //        act = false;
    //        gameObject.GetComponent<Collider>().enabled = false;
    //    }
    //}

    //public void RomperPared(Vector3 direccionEmpuje)
    //{
    //    paredRompida = true;

    //    // Desactivamos el collider del padre
    //    if (TryGetComponent<Collider>(out Collider col))
    //    {
    //        col.enabled = false;
    //    }

    //    foreach (Transform piedra in piedras)
    //    {
    //        // 1. Frenamos el temblor previo
    //        piedra.DOKill();

    //        // 2. Calculamos el punto de destino proyectado hacia adelante con dispersión aleatoria
    //        Vector3 dispersionAleatoria = Random.insideUnitSphere * dispersion;
    //        Vector3 destinoFinal = piedra.position + (direccionEmpuje.normalized * distanciaVuelo) + dispersionAleatoria;

    //        // 3. Creamos una Secuencia para combinar arco, rotación y desaparición
    //        Sequence secuenciaPiedra = DOTween.Sequence();

    //        // DOJump hace la trayectoria de parábola/disparo hacia el destino
    //        secuenciaPiedra.Join(piedra.DOJump(destinoFinal, alturaArco, 1, tiempoVuelo).SetEase(curvaVuelo));

    //        // Rotación aleatoria en 360 grados mientras vuela
    //        Vector3 girosAleatorios = new Vector3(
    //            Random.Range(180f, 540f),
    //            Random.Range(180f, 540f),
    //            Random.Range(180f, 540f)
    //        );
    //        secuenciaPiedra.Join(piedra.DORotate(girosAleatorios, tiempoVuelo, RotateMode.FastBeyond360).SetEase(Ease.Linear));

    //        // achicamos la piedra en el último 30% del trayecto para que desaparezca suave
    //        secuenciaPiedra.Insert(tiempoVuelo * 0.7f, piedra.DOScale(Vector3.zero, tiempoVuelo * 0.3f).SetEase(Ease.InQuad));

    //        // Destruimos el GameObject al terminar su animación individual
    //        secuenciaPiedra.OnComplete(() =>
    //        {
    //            //if (piedra != null) Destroy(piedra.gameObject);
    //        });
    //    }

    //    // Destruimos el objeto padre una vez que terminan las animaciones
    //    //Destroy(gameObject, tiempoVuelo + 0.1f);
    //}


    ////private void OnDestroy()
    ////{
    ////    // Limpieza preventiva de Tweens
    ////    foreach (Transform piedra in piedras)
    ////    {
    ////        if (piedra != null) piedra.DOKill();
    ////    }
    ////}



    //public void DetenerTemblor()
    //{
    //    if (tweenPosicion != null && tweenPosicion.IsActive()) tweenPosicion.Kill();
    //    if (tweenRotacion != null && tweenRotacion.IsActive()) tweenRotacion.Kill();
    //}



    //[SerializeField] GameObject desObj;

    //bool act;


    //private void Update()
    //{
    //    if (CheckpointManager.Instance.respawn)
    //    {
    //        desObj.SetActive(true);
    //        lavaObj.SetActive(true);
    //        //act = true;
    //        gameObject.GetComponent<Collider>().enabled = true;
    //        IniciarTemblorPiedras();
    //        for (int i = 0; i < piedras.Count; i++)
    //        {
    //            piedras[i].position = piedrasInicialPocision[i];
    //        }

    //    }

    //}

    ////private void OnTriggerStay(Collider collision)
    ////{
    ////    if ((ChooseElement(collision) || SearchAllElements(collision)) && desObj && !act)
    ////    {

    ////        desObj.SetActive(false);
    ////        act = false;
    ////        gameObject.GetComponent<Collider>().enabled = false;

    ////    }
    ////}



    //private bool ChooseElement(Collider other)
    //{
    //    if (other.gameObject.GetComponent<PlayerController>() != null)
    //        return other.gameObject.GetComponent<PlayerController>().energyPower;
    //    else return false;
    //}

    //private bool SearchAllElements(Collider other)
    //{

    //    if (other.gameObject.GetComponent<PlayerController>() != null) return other.gameObject.GetComponent<PlayerController>().energyPower;

    //    else return false;

    //}


}
