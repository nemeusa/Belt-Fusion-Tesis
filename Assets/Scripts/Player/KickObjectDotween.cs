using DG.Tweening;
using UnityEngine;

public class KickObjectDotween : MonoBehaviour, DashInteract
{
    [Header("Configuración de Explosión con DOTween")]
    [SerializeField] private float distanciaVuelo = 7f;
    [SerializeField] private float alturaArco = 2f;
    [SerializeField] private float tiempoVuelo = 1f;
    [SerializeField] private Ease curvaVuelo = Ease.OutQuad;

    Vector3 initialPos;
    Quaternion initialRot;
    Vector3 initialScale;

    private void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        initialScale = transform.localScale;  
    }

    private void Update()
    {
        if (CheckpointManager.Instance.respawn)
        {
            ResetAni();
        }
    }


    public void ExcuteAction(Collider target)
    {

        Vector3 direccionEmpuje = target.transform.forward;

        // Frenamos la rotación continua inmediatamente

        // Desactivamos el collider
        if (TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }

            // Animación de destrucción para cada pieza/bola

                //Vector3 dispersionAleatoria = Random.insideUnitSphere * dispersion;
                //Vector3 destinoFinal = transform.position + (direccionEmpuje.normalized * distanciaVuelo) + dispersionAleatoria;
                Vector3 destinoFinal = transform.position + (direccionEmpuje.normalized * distanciaVuelo);

                Sequence secuenciaPieza = DOTween.Sequence();

                // Salto parabólico
                secuenciaPieza.Join(transform.DOJump(destinoFinal, alturaArco, 1, tiempoVuelo).SetEase(curvaVuelo));

                // Giros descontrolados en el aire
                Vector3 girosAleatorios = new Vector3(
                    Random.Range(180f, 540f),
                    Random.Range(180f, 540f),
                    Random.Range(180f, 540f)
                );
                secuenciaPieza.Join(transform.DORotate(girosAleatorios, tiempoVuelo, RotateMode.FastBeyond360).SetEase(Ease.Linear));

                // Achicar hasta desaparecer al final del trayecto
                secuenciaPieza.Insert(tiempoVuelo * 0.7f, transform.DOScale(Vector3.zero, tiempoVuelo * 0.3f).SetEase(Ease.InQuad));
            
        

    }

    public void ResetAni()
    {
        transform.DOKill(); // Matamos animaciones anteriores de la piedra
        transform.position = initialPos;
        transform.rotation = initialRot;
        transform.localScale = initialScale;
    }    
}
