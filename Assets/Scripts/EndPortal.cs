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

    private void Awake()
    {
        //playerPos = GameManager.instance.player.transform;

        if (globalVolume.profile.TryGet<PaniniProjection>(out var tmpPanini)) panini = tmpPanini;
        if (globalVolume.profile.TryGet<Bloom>(out var tmpBloom)) bloom = tmpBloom;
        if (globalVolume.profile.TryGet<Vignette>(out var tmpVignette)) vignette = tmpVignette;

    }

    void Update()
    {
        if (Vector3.Distance(playerPos.position, transform.position) < 5)
        {

            panini.distance.value = 1;
            bloom.intensity.value = 5;
            vignette.intensity.value = 5;
            Debug.Log("si");
        }
    }

}
