using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SpinBallEffect : MonoBehaviour, DashInteract
{
    [Header("Referencias")]
    [Tooltip("Poner aquí la bola o los pedazos si se divide en varias partes. Si se deja vacío, usa este mismo objeto.")]
    [SerializeField] private List<Transform> piezasBola = new List<Transform>();
    [SerializeField] private GameObject lavaObj;
    [SerializeField] private GameObject desObj;

    [Header("Configuración del Rodado")]
    [SerializeField] private Vector3 ejeRotacion = new Vector3(1f, 0f, 0f); // Eje sobre el que rueda
    [SerializeField] private float tiempoPorVuelta = 0.4f;

    [Header("Configuración de Explosión con DOTween")]
    [SerializeField] private float distanciaVuelo = 7f;
    [SerializeField] private float alturaArco = 2f;
    [SerializeField] private float dispersion = 1.5f;
    [SerializeField] private float tiempoVuelo = 1f;
    [SerializeField] private Ease curvaVuelo = Ease.OutQuad;

    // Listas para guardar el estado inicial completo
    private List<Vector3> piezasInicialPosicion = new List<Vector3>();
    private List<Quaternion> piezasInicialRotacion = new List<Quaternion>();
    private List<Vector3> piezasInicialEscala = new List<Vector3>();

    private Tween tweenRodado;
    private bool bolaRompida = false;

    public bool destroyBall;

    private void Start()
    {
        // Si no asignaste piezas en el inspector, usamos el transform de este mismo objeto
        if (piezasBola.Count == 0)
        {
            piezasBola.Add(transform);
        }

        // Guardamos posición, rotación y escala iniciales
        if (destroyBall)
            foreach (var p in piezasBola)
            {
                piezasInicialPosicion.Add(p.position);
                piezasInicialRotacion.Add(p.rotation);
                piezasInicialEscala.Add(p.localScale);
            }

        IniciarRodado();
    }

    //private void Update()
    //{
    //    if (CheckpointManager.Instance.respawn)
    //    {
    //        ResetBola();
    //    }
    //}

    public void ExcuteAction(Collider target)
    {

        if (bolaRompida || !destroyBall) return;


        //gameObject.GetComponent<Rigidbody>() = null;

        if (lavaObj != null) lavaObj.SetActive(false);

        Vector3 direccionEmpuje = target.transform.forward;
        RomperBola(direccionEmpuje);

    }

    public void DontExecute(Collider target)
    {
        GameManager.instance.Death(target.gameObject, 0.2f, null, null);
    }

    private void IniciarRodado()
    {
        DetenerAnimaciones();

        // Giro continuo infinito
        tweenRodado = transform.DORotate(ejeRotacion * 360f, tiempoPorVuelta, RotateMode.WorldAxisAdd)
                               .SetEase(Ease.Linear)
                               .SetLoops(-1, LoopType.Incremental);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (bolaRompida || !destroyBall) return;

    //    if (other.GetComponent<PlayerController>() != null)
    //    {

    //        //gameObject.GetComponent<Rigidbody>() = null;

    //        if (lavaObj != null) lavaObj.SetActive(false);

    //        Vector3 direccionEmpuje = other.transform.forward;
    //        RomperBola(direccionEmpuje);
    //    }
    //}

    public void RomperBola(Vector3 direccionEmpuje)
    {
        bolaRompida = true;

        // Frenamos la rotación continua inmediatamente
        DetenerAnimaciones();

        // Desactivamos el collider
        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        // Animación de destrucción para cada pieza/bola
        foreach (Transform pieza in piezasBola)
        {
            Vector3 dispersionAleatoria = Random.insideUnitSphere * dispersion;
            Vector3 destinoFinal = pieza.position + (direccionEmpuje.normalized * distanciaVuelo) + dispersionAleatoria;

            Sequence secuenciaPieza = DOTween.Sequence();

            // Salto parabólico
            secuenciaPieza.Join(pieza.DOJump(destinoFinal, alturaArco, 1, tiempoVuelo).SetEase(curvaVuelo));

            // Giros descontrolados en el aire
            Vector3 girosAleatorios = new Vector3(
                Random.Range(180f, 540f),
                Random.Range(180f, 540f),
                Random.Range(180f, 540f)
            );
            secuenciaPieza.Join(pieza.DORotate(girosAleatorios, tiempoVuelo, RotateMode.FastBeyond360).SetEase(Ease.Linear));

            // Achicar hasta desaparecer al final del trayecto
            secuenciaPieza.Insert(tiempoVuelo * 0.7f, pieza.DOScale(Vector3.zero, tiempoVuelo * 0.3f).SetEase(Ease.InQuad));
        }
    }

    public void ResetBola()
    {
        bolaRompida = false;

        if (desObj != null) desObj.SetActive(true);
        if (lavaObj != null) lavaObj.SetActive(true);
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = true;

        // Restauramos transformaciones iniciales
        for (int i = 0; i < piezasBola.Count; i++)
        {
            piezasBola[i].DOKill();
            piezasBola[i].position = piezasInicialPosicion[i];
            piezasBola[i].rotation = piezasInicialRotacion[i];
            piezasBola[i].localScale = piezasInicialEscala[i];
        }

        // Volvemos a iniciar el giro continuo
        IniciarRodado();
    }

    public void DetenerAnimaciones()
    {
        if (tweenRodado != null && tweenRodado.IsActive())
        {
            tweenRodado.Kill();
        }

        foreach (Transform pieza in piezasBola)
        {
            if (pieza != null) pieza.DOKill();
        }
    }

    private void OnDestroy()
    {
        DetenerAnimaciones();
    }


}
