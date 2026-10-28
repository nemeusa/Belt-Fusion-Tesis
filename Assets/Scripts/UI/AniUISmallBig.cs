using DG.Tweening;
using UnityEngine;

public class AniUISmallBig : MonoBehaviour
{
    [Header("Configuración del Pulso")]
    [Tooltip("Escala máxima a la que llega (Ej: 1.2 es un 20% más grande)")]
    [SerializeField] private float escalaMaxima = 1.2f;

    [Tooltip("Tiempo que tarda en agrandarse (el ciclo completo de ida y vuelta dura el doble)")]
    [SerializeField] private float duracionPulso = 0.5f;

    [SerializeField] private Ease curvaPulso = Ease.InOutSine;

    [Header("Opciones Extras")]
    [Tooltip("Marcar si querés que funcione aunque el juego esté pausado (Time.timeScale = 0)")]
    //[SerializeField] private bool ignorarPausa = true;

    private Tween tweenPulso;


    private void Start()
    {
        IniciarPulso();
    }

    public void IniciarPulso()
    {
        DetenerPulso();

        // Guardamos la escala base por si cambia, pero DOScale toma el tamaño actual como inicio
        tweenPulso = transform.DOScale(escalaMaxima, duracionPulso)
                              .SetEase(curvaPulso)
                              .SetLoops(-1, LoopType.Yoyo); // -1 para infinito, Yoyo para ida y vuelta

        //// Si es para un menú de pausa, esto evita que se congele cuando el juego se detiene
        //if (ignorarPausa)
        //{
        //    tweenPulso.SetUpdate(true);
        //}
    }

    public void DetenerPulso()
    {
        if (tweenPulso != null && tweenPulso.IsActive())
        {
            tweenPulso.Kill();
        }
    }


    private void OnEnable()
    {
        IniciarPulso();
    }

    private void OnDisable()
    {
        DetenerPulso();
    }

    private void OnDestroy()
    {
        DetenerPulso();
    }
}
