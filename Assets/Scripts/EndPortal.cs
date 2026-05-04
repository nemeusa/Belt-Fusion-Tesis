using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EndPortal : MonoBehaviour
{
    public Volume globalVolume;
    private PaniniProjection panini;
    Bloom bloom;
    Vignette vignette;
    public Transform playerPos;

    [SerializeField] float distanceEffects = 2;

    float panDef;
    float blomDef;
    float vigDef;

    private void Start()
    {
        //playerPos = GameManager.instance.player.transform;

        if (globalVolume.profile.TryGet<PaniniProjection>(out var tmpPanini)) panini = tmpPanini;
        if (globalVolume.profile.TryGet<Bloom>(out var tmpBloom)) bloom = tmpBloom;
        if (globalVolume.profile.TryGet<Vignette>(out var tmpVignette)) vignette = tmpVignette;


        panDef = panini.distance.value;
        blomDef = bloom.intensity.value;
        vigDef = vignette.intensity.value;

    }

    void Update()
    {
        Por();
        //Debug.Log(Vector3.Distance(transform.position, playerPos.position));
        //if (Vector3.Distance(transform.position, playerPos.position) > distanceEffects)
        //{


        //    panini.distance.value = 1;
        //    bloom.intensity.value = 5;
        //    vignette.intensity.value = 5;
        //    Debug.Log("si");
        //}
        //else
        //{
        //    panini.distance.value = panDef;
        //    bloom.intensity.value = blomDef;
        //    vignette.intensity.value = vigDef;
        //}

    }

    void Por()
    {
        float currentDistance = Vector3.Distance(playerPos.position, transform.position);

        // 2. Creamos un factor de 0 a 1
        // Si estamos lejos ( > distanceEffects), el factor es 0.
        // Si estamos encima del portal (distancia 0), el factor es 1.
        float factor = 1f - Mathf.Clamp01(currentDistance / distanceEffects);

        // 3. Aplicamos los efectos usando el factor para hacer una transición suave (Lerp)
        // Mathf.Lerp(valor_inicial, valor_maximo, factor)

        panini.distance.value = Mathf.Lerp(panDef, 1f, factor);
        bloom.intensity.value = Mathf.Lerp(blomDef, 15f, factor); // Subí el 5 a 15 para que se note el "punch"
        vignette.intensity.value = Mathf.Lerp(vigDef, 0.5f, factor); // Ojo: Vignette 5 es pantalla negra total, 0.5 es mejor

        // Debug para que veas el progreso en consola
        Debug.Log("Intensidad del Portal: " + (factor * 100).ToString("F0") + "%");
    }

}
