using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Configuración del Bullet Time")]
    [Range(0f, 1f)] public float factorRalentizacion = 0.2f; // El mundo va al 20% de velocidad
    public float duracionEfecto = 2f; // Cuánto dura en segundos reales

    [SerializeField] ActiveFilters activeFiltersCode;

    private bool _efectoActivo = false;

    Vignette vignette;
    ChromaticAberration aberration;
    LensDistortion distortion;



    private void Awake() => Instance = this;

    private void Start()
    {
        activeFiltersCode.AlternarFiltroBlancoNegro(false);
    }

    public void ActivarCamaraLenta()
    {
        if (!_efectoActivo)
        {
            if (GameManager.instance.player.globalVolume.profile.TryGet<Vignette>(out var vignetteTmp))
                vignette = vignetteTmp;

            if (GameManager.instance.player.globalVolume.profile.TryGet<ChromaticAberration>(out var aberrationTmp)) aberration = aberrationTmp;

            if (GameManager.instance.player.globalVolume.profile.TryGet<LensDistortion>(out var distortionTmp)) distortion = distortionTmp;



            StartCoroutine(BulletTimeRoutine());

            StartCoroutine(EffectsCongelation());


        }
    }

    private IEnumerator BulletTimeRoutine()
    {
        //cambia la velocidad del player cuando se mueve con tiempo normal
        //GameManager.instance.player.speed *= 3;
        //GameManager.instance.player._gravityValue *= 2;

        activeFiltersCode.AlternarFiltroBlancoNegro(true);

        _efectoActivo = true;

        // Bajamos el tiempo global
        Time.timeScale = factorRalentizacion;

        // IMPORTANTÍSIMO: Ajustamos el fixedDeltaTime en proporción 
        // para que las físicas (fuerzas, colisiones) no se vuelvan locas o vayan a saltos
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Esperamos en "tiempo real" (ignora el timeScale actual)
        yield return new WaitForSecondsRealtime(duracionEfecto);

        activeFiltersCode.AlternarFiltroBlancoNegro(false);
        // Volvemos a la normalidad progresivamente
        while (Time.timeScale < 1f)
        {
            Time.timeScale += Time.unscaledDeltaTime * 2f; // Velocidad de recuperación
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            //cambia la velocidad del player cuando se mueve con tiempo normal
            //GameManager.instance.player.speed = GameManager.instance.player.initialSpeed;
            //GameManager.instance.player._gravityValue *= 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            yield return null;
        }

        _efectoActivo = false;
    }


    IEnumerator EffectsCongelation()
    {
        // 1. Seteamos los valores iniciales del efecto
        vignette.intensity.value = 0.2f;
        vignette.color.value = Color.cyan;
        vignette.rounded.value = true;
        vignette.smoothness.value = 0.5f;
        aberration.intensity.value = 1;
        distortion.intensity.value = -0.5f;

        // 2. Esperamos el tiempo que el efecto está "al máximo"
        yield return new WaitForSeconds(duracionEfecto - 2);

        // 3. Transición suave (Fade Out)
        float duration = 1.0f; // Duración del suavizado en segundos
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Interpolamos los valores de los efectos hacia su estado original (0)
            distortion.intensity.value = Mathf.Lerp(-0.5f, 0f, t);
            aberration.intensity.value = Mathf.Lerp(1f, 0f, t);
            vignette.intensity.value = Mathf.Lerp(0.2f, 0f, t);
            vignette.smoothness.value = Mathf.Lerp(0.5f, 1f, t);

            yield return null; // Espera al siguiente frame
        }

        // 4. Aseguramos los valores finales y limpieza
        distortion.intensity.value = 0;
        aberration.intensity.value = 0;
        vignette.intensity.value = 0f;
        vignette.rounded.value = false;

        //vignette.intensity.value = 0.2f;
        //vignette.color.value = Color.yellow;
        //vignette.rounded.value = true;
        //vignette.smoothness.value = 0.5f;
        //aberration.intensity.value = 1;
        //distortion.intensity.value = -0.5f;

        //yield return new WaitForSeconds(1f);
        //distortion.intensity.value = 0;
        //aberration.intensity.value = 0;
        //vignette.intensity.value = 0f;
        //vignette.rounded.value = false;
        //vignette.smoothness.value = 1f;
    }

    private void OnDisable()
    {
        activeFiltersCode.AlternarFiltroBlancoNegro(false);
    }
}
