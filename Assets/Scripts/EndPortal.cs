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

    }

    void Por()
    {
        float currentDistance = Vector3.Distance(playerPos.position, transform.position);

        if (currentDistance > distanceEffects) return;

        vignette.color.value = Color.blue;

        float factor = 1f - Mathf.Clamp01(currentDistance / distanceEffects);


        panini.distance.value = Mathf.Lerp(panDef, 1f, factor);
        bloom.intensity.value = Mathf.Lerp(blomDef, 15f, factor); 
        vignette.intensity.value = Mathf.Lerp(vigDef, 0.5f, factor); 

        //Debug.Log("Intensidad del Portal: " + (factor * 100).ToString("F0") + "%");
    }

}
