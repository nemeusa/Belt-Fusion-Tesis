using System;
using System.Collections;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public static TimeSlow Instance;

    public static event Action<float> OnTimeSlowed; // Pasa el factor de velocidad (ej: 0.2f)
    public static event Action OnTimeNormalized;

    [Header("Configuración")]
    [Range(0f, 1f)] public float factorRalentizacion = 0.2f;

    public float duracionEfecto = 2f; // Cuánto dura en segundos reales

    [SerializeField] ActiveFilters activeFiltersCode;



    private void Awake()
    {
        Instance = this;
    }


    public IEnumerator BulletTimeRoutine()
    {

        activeFiltersCode.AlternarFiltroBlancoNegro(true);

        ActivarHabilidadTiempo();

        yield return new WaitForSecondsRealtime(duracionEfecto);

        activeFiltersCode.AlternarFiltroBlancoNegro(false);

        DesactivarHabilidadTiempo();


    }


    // Llamá a esto cuando el jugador presione el botón de la habilidad
    public void ActivarHabilidadTiempo()
    {
        // Avisamos a todas las trampas que sintonicen el factor de cámara lenta
        OnTimeSlowed?.Invoke(factorRalentizacion);
        Debug.Log("Habilidad activada: Trampas ralentizadas.");
    }

    // Llamá a esto cuando termine la habilidad
    public void DesactivarHabilidadTiempo()
    {
        // Avisamos a todas las trampas que vuelvan a su velocidad normal
        OnTimeNormalized?.Invoke();
        Debug.Log("Habilidad terminada: Tiempo normal.");
    }


    private void OnDisable()
    {
        activeFiltersCode.AlternarFiltroBlancoNegro(false);
    }
}
